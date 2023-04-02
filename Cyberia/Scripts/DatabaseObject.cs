using SQLite;

using System.Reflection;

namespace Cyberia.Scripts
{
    public readonly struct DatabaseObject
    {
        public LangObject LangObject { get; init; }
        public List<DatabaseProperty> UpdatedProperties { get; init; }

        public DatabaseObject(LangObject langObject)
        {
            LangObject = langObject;
            UpdatedProperties = new();
        }

        public DatabaseObject(DatabaseObject other)
        {
            LangObject = other.LangObject;
            UpdatedProperties = new(other.UpdatedProperties);
        }

        public void SetUpdatedPropertiesFromParent(DatabaseObject parent)
        {
            foreach (KeyValuePair<string, LangObject> property in LangObject.ValueProperties)
            {
                if (!string.IsNullOrEmpty(property.Value.ParentDbName))
                {
                    DatabaseProperty parentProperty = parent.UpdatedProperties.Find(x => x.LangProperty.DbName.Equals(property.Value.ParentDbName));
                    UpdatedProperties.Add(new(property.Value, parentProperty.Value));
                }
            }
        }

        public async Task<bool> InsertOrUpdate()
        {
            Type? tableType = LangObject.GetTableType();
            if (tableType is null)
                return false;

            TableMapping table = new(tableType);

            object dbObject;
            bool isNew = true;

            string pkValue = GetGeneratedPrimaryKeyValue(out int nbUniqueKey);
            try
            {
                dbObject = await Cyberia.Api.Database.GetAsync(pkValue, table);
                isNew = false;
            }
            catch
            {
                dbObject = Activator.CreateInstance(tableType)!;

                if (nbUniqueKey > 1)
                    SetPropertyValue(dbObject, tableType, "Id", typeof(string), pkValue);
            }

            foreach (DatabaseProperty updatedProperty in UpdatedProperties)
            {
                if (!updatedProperty.LangProperty.Ignored)
                    SetPropertyValue(dbObject, tableType, updatedProperty.LangProperty.DbName, updatedProperty.LangProperty.GetValueType(), updatedProperty.Value);
            }

            if (isNew)
                await Cyberia.Api.Database.InsertAsync(dbObject);
            else
                await Cyberia.Api.Database.UpdateAsync(dbObject);

            return true;
        }

        private string GetGeneratedPrimaryKeyValue(out int nbUniqueKey)
        {
            List<string> values = new();

            nbUniqueKey = 0;
            foreach (DatabaseProperty property in UpdatedProperties)
            {
                if (property.LangProperty.UniqueKey)
                {
                    values.Add(property.Value);
                    nbUniqueKey++;
                }
            }

            return string.Join('_', values);
        }

        private static bool SetPropertyValue(object obj, Type tableType, string propertyName, Type propertyType, string value)
        {
            PropertyInfo? property = tableType.GetProperty(propertyName);
            if (property is null)
            {
                Cyberia.Logger.Error($"Unknown {propertyName} property in {tableType.Name} class");
                return false;
            }

            try
            {
                if (!value.Equals("null"))
                    property.SetValue(obj, Convert.ChangeType(value, propertyType));
            }
            catch (Exception e)
            {
                Cyberia.Logger.Error(e);
                return false;
            }

            return true;
        }
    }
}
