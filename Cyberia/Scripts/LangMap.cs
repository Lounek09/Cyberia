using Cyberia.Api.Datacenter;

using SQLite;

using System.Reflection;
using System.Text.Json.Serialization;

namespace Cyberia.Scripts
{
    public sealed class LangObject
    {
        [JsonPropertyName("DbName")]
        public string DbName { get; init; }

        [JsonPropertyName("ParentDbName")]
        public string ParentDbName { get; init; }

        [JsonPropertyName("Table")]
        public bool Table { get; init; }

        [JsonPropertyName("Keys")]
        public List<LangObject> KeyProperties { get; init; }

        [JsonPropertyName("Type")]
        public string Type { get; init; }

        [JsonPropertyName("RealType")]
        public string RealType { get; init; }

        [JsonPropertyName("Values")]
        public Dictionary<string, LangObject> ValueProperties { get; init; }

        [JsonPropertyName("UniqueKey")]
        public bool UniqueKey { get; init; }

        [JsonPropertyName("Ignored")]
        public bool Ignored { get; init; }

        public LangObject()
        {
            DbName = string.Empty;
            ParentDbName = string.Empty;
            KeyProperties = new();
            Type = string.Empty;
            RealType = string.Empty;
            ValueProperties = new();
        }

        public Type? GetTableType()
        {
            if (Table)
            {
                Assembly? assembly = Assembly.GetAssembly(typeof(ITable));
                if (assembly is not null)
                {
                    foreach (Type type in assembly.GetTypes())
                    {
                        IEnumerable<Attribute> attributes = type.GetCustomAttributes();
                        Attribute? attribute = attributes.FirstOrDefault(p => p is TableAttribute);

                        if (attribute is not null && attribute is TableAttribute tableAttribute && tableAttribute.Name.Equals(DbName))
                            return type;
                    }
                }
            }

            Cyberia.Logger.Error($"Unknown {DbName} table");
            return null;
        }

        public Type GetValueType()
        {
            string type = string.IsNullOrEmpty(RealType) ? Type : RealType;

            switch (type)
            {
                case "Integer":
                    return typeof(int);
                case "Double":
                    return typeof(double);
                case "Boolean":
                    return typeof(bool);
                default:
                    return typeof(string);
            }
        }
    }
}
