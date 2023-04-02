using Cyberia.Langs.Enums;

using System.Text.RegularExpressions;

namespace Cyberia.Scripts
{
    public static class DatabaseBuilder
    {
        private const string LANGSMAP_PATH = $"api/langsMap.json";
        private const string KEY_VALUE_SEPARATOR = " = ";

        private static readonly string[] _ignoredLines = new string[] { "new Object();", "new Array();" };

        public static async Task<bool> Launch(LangType type, Language language)
        {
            Dictionary<string, Dictionary<string, LangObject>> map = Json.LoadFromFile<Dictionary<string, Dictionary<string, LangObject>>>(LANGSMAP_PATH);

            string langPath = $"{Constant.LANGS_PATH}/{type.ToString().ToLower()}/{language.ToString().ToLower()}";
            if (!Directory.Exists(langPath))
            {
                Cyberia.Logger.Error($"Unknown '{langPath}' directory when building the database");
                return false;
            }

            foreach (KeyValuePair<string, Dictionary<string, LangObject>> keyValueMap in map.Where(x => x.Key.Equals("ranks")))
            {
                string name = keyValueMap.Key;
                Dictionary<string, LangObject> objects = keyValueMap.Value;

                string extractedLangFilePath = $"{langPath}/{name}/current.as";
                if (!File.Exists(extractedLangFilePath))
                {
                    Cyberia.Logger.Error($"There is no extracted lang for '{name}' lang in {language.ToString().ToLower()}");
                    return false;
                }

                Cyberia.Logger.Debug($"Start parsing '{name}' lang");

                string[] lines = File.ReadAllLines(extractedLangFilePath);
                if (!await ParseLines(lines, objects))
                {
                    Cyberia.Logger.Error($"Error when parsing '{name}' lang");
                    return false;
                }
            }

            return true;
        }

        private static async Task<bool> ParseLines(string[] lines, Dictionary<string, LangObject> langObjects)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                if (_ignoredLines.Any(x => line.EndsWith(x)))
                {
                    Cyberia.Logger.Debug($"Skip line {i + 1} : {line}");
                    continue;
                }

                Cyberia.Logger.Debug($"Start line {i + 1} : {line}");

                if (!await ParseLine(line, langObjects))
                {
                    Cyberia.Logger.Error($"Error in line {i + 1} : {line}");
                    return false;
                }
            }

            return true;
        }

        private static async Task<bool> ParseLine(string line, Dictionary<string, LangObject> langObjects)
        {
            string[] lineSplit = line.Split(KEY_VALUE_SEPARATOR, 2);
            if (lineSplit.Length < 2)
                return false;

            string lineKey = lineSplit[0];
            string lineValue = lineSplit[1].Replace("' + '\"' + '", "\"");

            if (!ParseLine_GetLangObjectName(lineKey, out string langObjectName, out int pos))
            {
                Cyberia.Logger.Error($"Error when getting the object name");
                return false;
            }

            if (!langObjects.TryGetValue(langObjectName, out LangObject? langObject))
            {
                Cyberia.Logger.Error($"Unknown object name '{langObjectName}'");
                return false;
            }

            if (langObject.Ignored)
                return true;

            DatabaseObject dbObject = new(langObject);

            if (langObject.KeyProperties.Count > 0)
            {
                if (!ParseLine_Keys(lineKey, dbObject, pos))
                    return false;
            }

            if (langObject.ValueProperties.Count > 0)
            {
                if (!await ParseLine_Value(lineValue, dbObject))
                    return false;
            }

            return true;
        }

        //Key
        private static bool ParseLine_GetLangObjectName(string line, out string objectName, out int pos)
        {
            char endChar = '\0';
            if (line.Contains('['))
                endChar = '[';
            else if (line.Contains('.'))
                endChar = '.';

            objectName = "";
            for (pos = 0; pos < line.Length && !line[pos].Equals(endChar); pos++)
                objectName += line[pos];

            pos++;
            return true;
        }

        private static bool ParseLine_Keys(string line, DatabaseObject dbObject, int pos)
        {
            foreach (LangObject property in dbObject.LangObject.KeyProperties)
            {
                if (!ParseLine_GetKeyValue(line, out string value, ref pos))
                    return false;

                dbObject.UpdatedProperties.Add(new(property, value));
            }

            return true;
        }

        private static bool ParseLine_GetKeyValue(string line, out string keyValue, ref int pos)
        {
            bool isNumber = line.Contains(']');

            keyValue = "";
            for (; pos < line.Length && line[pos] is not ']'; pos++)
            {
                if (isNumber && pos + 1 == line.Length)
                    return false;

                keyValue += line[pos];
            }

            pos++;
            return true;
        }

        //Value
        private static async Task<bool> ParseLine_Value(string line, DatabaseObject dbObject)
        {
            if (line.Equals("null"))
                return true;

            switch (dbObject.LangObject.Type)
            {
                case "Object":
                    return await ParseLine_Value_Object(line, dbObject);
                case "Array":
                    return await ParseLine_Value_Array(line, dbObject);
                case "ArrayIndexValue":
                    return await ParseLine_Value_ArrayIndexValue(line, dbObject);
                case "ArrayMap":
                    return await ParseLine_Value_ArrayMap(line, dbObject);
                case "ArrayProperty":
                    return await ParseLine_Value_ArrayProperty(line, dbObject);
                case "Value":
                    return await ParseLine_Value_Value(line, dbObject);
                default:
                    Cyberia.Logger.Error($"Unknow line value type '{dbObject.LangObject.Type}'");
                    return false;
            }
        }

        private static async Task<bool> ParseLine_Value_Object(string line, DatabaseObject dbObject)
        {
            //{'n': 'Neutre', 'c': true};
            //{'n': 'Neutre', 'd': '-', 'o': 0, 'av': 0, 'f': []};
            int pos = 0;
            while (line[pos] is not '}')
            {
                pos++;

                if (!ParseLine_Property_Object_PropertyName(line, out string propertyName, out string? propertyNamePotentialValue, ref pos))
                    return false;

                dbObject.LangObject.ValueProperties.TryGetValue(propertyName, out LangObject? property);
                if (property is null)
                {
                    Cyberia.Logger.Error($"Unknow '{propertyName}' property");
                    return false;
                }

                pos += 2;

                if (!ParseLine_Property(line, property.Type, out string propertyValue, ref pos))
                    return false;

                if (property.Table)
                {
                    DatabaseObject subDbObject = new(property);
                    subDbObject.SetUpdatedPropertiesFromParent(dbObject);

                    if (propertyNamePotentialValue is not null)
                        subDbObject.UpdatedProperties.Add(new(property.ValueProperties["#"], propertyNamePotentialValue));

                    await ParseLine_Value(propertyValue, subDbObject);
                }
                else
                    dbObject.UpdatedProperties.Add(new(property, propertyValue));

                if (line[pos] is not '}')
                    pos++;
            }

            return await dbObject.InsertOrUpdate();
        }

        private static async Task<bool> ParseLine_Value_Array(string line, DatabaseObject dbObject)
        {
            //[697, 698, 699]
            if (line.Equals("[]") || line.Equals("[null]"))
                return true;

            LangObject property = dbObject.LangObject.ValueProperties["value"];

            int pos = 0;
            for (int i = 0; line[pos] is not ']'; i++)
            {
                DatabaseObject arrayDbObject = new(dbObject);

                pos++;

                if (!ParseLine_Property(line, property.Type, out string propertyValue, ref pos))
                    return false;

                if (property.Table)
                {
                    DatabaseObject subDbObject = new(property);
                    subDbObject.SetUpdatedPropertiesFromParent(arrayDbObject);
                    await ParseLine_Value(propertyValue, subDbObject);
                }
                else
                    arrayDbObject.UpdatedProperties.Add(new(property, propertyValue));

                if (!await arrayDbObject.InsertOrUpdate())
                    return false;

                if (line[pos] is not ']')
                    pos++;
            }

            return true;
        }

        private static async Task<bool> ParseLine_Value_ArrayIndexValue(string line, DatabaseObject dbObject)
        {
            //[true, false, false, false];
            if (line.Equals("[]") || line.Equals("[null]"))
                return true;

            LangObject keyProperty = dbObject.LangObject.ValueProperties["index"];
            LangObject valueProperty = dbObject.LangObject.ValueProperties["value"];

            int pos = 0;
            for (int i = 0; line[pos] is not ']'; i++)
            {
                DatabaseObject arrayDbObject = new(dbObject);

                arrayDbObject.UpdatedProperties.Add(new(keyProperty, i.ToString()));

                pos++;

                if (!ParseLine_Property(line, valueProperty.Type, out string propertyValue, ref pos))
                    return false;

                if (valueProperty.Table)
                {
                    DatabaseObject subDbObject = new(valueProperty);
                    subDbObject.SetUpdatedPropertiesFromParent(arrayDbObject);
                    await ParseLine_Value(propertyValue, subDbObject);
                }
                else
                    arrayDbObject.UpdatedProperties.Add(new(valueProperty, propertyValue));

                if (!await arrayDbObject.InsertOrUpdate())
                    return false;

                if (line[pos] is not ']')
                    pos++;
            }

            return true;
        }

        private static async Task<bool> ParseLine_Value_ArrayMap(string line, DatabaseObject dbObject)
        {
            //[[441, 4], [446, 3], [442, 3], [444, 1]];
            if (line.Equals("[]") || line.Equals("[null]"))
                return true;

            int pos = 1;
            for (int i = 0; line[pos] is not ']'; i++)
            {
                DatabaseObject arrayDbObject = new(dbObject);

                pos++;

                LangObject property = arrayDbObject.LangObject.ValueProperties["key"];

                if (!ParseLine_Property(line, property.Type, out string propertyValue, ref pos))
                    return false;

                arrayDbObject.UpdatedProperties.Add(new(property, propertyValue));

                pos += 2;

                property = arrayDbObject.LangObject.ValueProperties["value"];

                if (!ParseLine_Property(line, property.Type, out propertyValue, ref pos))
                    return false;

                arrayDbObject.UpdatedProperties.Add(new(property, propertyValue));

                if (!await arrayDbObject.InsertOrUpdate())
                    return false;

                pos++;

                if (line[pos] is not ']')
                    pos += 2;
            }

            return true;
        }

        private static async Task<bool> ParseLine_Value_ArrayProperty(string line, DatabaseObject dbObject)
        {
            //[5, 4, 1, 1, 50, 30, false, false]
            if (line.Equals("[]") || line.Equals("[null]"))
                return true;

            int pos = 0;
            for (int i = 0; line[pos] is not ']'; i++)
            {
                DatabaseObject arrayDbObject = new(dbObject);

                dbObject.LangObject.ValueProperties.TryGetValue($"value{i}", out LangObject? property);
                if (property is null)
                {
                    Cyberia.Logger.Error($"Unknow 'value{i}' property");
                    return false;
                }

                pos++;

                if (!ParseLine_Property(line, property.Type, out string propertyValue, ref pos))
                    return false;

                if (property.Table)
                {
                    DatabaseObject subDbObject = new(property);
                    subDbObject.SetUpdatedPropertiesFromParent(arrayDbObject);
                    await ParseLine_Value(propertyValue, subDbObject);
                }
                else
                    arrayDbObject.UpdatedProperties.Add(new(property, propertyValue));

                if (!await arrayDbObject.InsertOrUpdate())
                    return false;

                if (line[pos] is not ']')
                    pos++;
            }

            return true;
        }

        private static async Task<bool> ParseLine_Value_Value(string line, DatabaseObject dbObject)
        {
            //'+#1 en portée';
            int pos = 0;

            LangObject property = dbObject.LangObject.ValueProperties["value"];

            if (!ParseLine_Property(line, property.Type, out string propertyValue, ref pos))
                return false;

            dbObject.UpdatedProperties.Add(new(property, propertyValue));

            return await dbObject.InsertOrUpdate();
        }

        //Property
        private static bool ParseLine_Property_Object_PropertyName(string line, out string propertyName, out string? propertyNamePotentialValue, ref int pos)
        {
            propertyName = "";
            propertyNamePotentialValue = null;
            for (pos++; line[pos] is not '\''; pos++)
            {
                if (pos + 1 == line.Length)
                    return false;

                propertyName += line[pos];
            }

            Match match = Regex.Match(propertyName, @"(\D+)(\d+)");
            if (match.Success)
            {
                propertyName = match.Groups[1].Value + "#";
                propertyNamePotentialValue = match.Groups[2].Value;
            }


            pos++;
            return true;
        }

        private static bool ParseLine_Property(string line, string propertyType, out string propertyValue, ref int pos)
        {
            if (line[pos] is 'n')
            {
                pos += 4;
                propertyValue = "null";
                return true;
            }

            switch (propertyType)
            {
                case "String":
                    return ParseLine_Property_String(line, out propertyValue, ref pos);
                case "Object":
                case "Array":
                case "ArrayIndexValue":
                case "ArrayMap":
                case "ArrayProperty":
                    return ParseLine_Property_ObjectOrArray(line, out propertyValue, ref pos);
                case "Unknown":
                    switch (line[pos])
                    {
                        case '\'':
                            return ParseLine_Property_String(line, out propertyValue, ref pos);
                        case '[':
                        case '{':
                            return ParseLine_Property_ObjectOrArray(line, out propertyValue, ref pos);
                        default:
                            return ParseLine_Property_Others(line, out propertyValue, ref pos);
                    }
                default:
                    return ParseLine_Property_Others(line, out propertyValue, ref pos);
            }
        }

        private static bool ParseLine_Property_String(string line, out string propertyValue, ref int pos)
        {
            propertyValue = "";
            for (pos++; line[pos] is not '\''; pos++)
            {
                if (pos + 1 == line.Length)
                    return false;

                if (line[pos] is '\\' && line[pos + 1] is '\'')
                    pos++;

                propertyValue += line[pos];
            }

            pos++;
            return true;
        }

        private static bool ParseLine_Property_ObjectOrArray(string line, out string propertyValue, ref int pos)
        {
            int level = 1;

            propertyValue = line[pos].ToString();
            for (pos++; level > 0; pos++)
            {
                if (pos + 1 == line.Length)
                    return false;

                if (line[pos] is '[' or '{')
                    level++;
                else if (line[pos] is ']' or '}')
                    level--;

                propertyValue += line[pos];
            }

            return true;
        }

        private static bool ParseLine_Property_Others(string line, out string propertyValue, ref int pos)
        {
            propertyValue = "";
            for (; line[pos] is not ',' && line[pos] is not '}' && line[pos] is not ']' && line[pos] is not ';'; pos++)
            {
                if (pos + 1 == line.Length)
                    return false;

                propertyValue += line[pos];
            }

            return true;
        }
    }
}
