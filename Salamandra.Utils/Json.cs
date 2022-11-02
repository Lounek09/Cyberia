using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Salamandra.Utils
{
    public static class Json
    {
        public static T Load<T>(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);

                return JsonSerializer.Deserialize<T>(json)!;
            }
            catch (Exception e) when (e is FileNotFoundException or DirectoryNotFoundException)
            {
                Console.WriteLine($"File not found for deserialization '{Path.GetFileName(filePath)}'\n{e.Message}");
            }
            catch (Exception e) when (e is JsonException or NotSupportedException or InvalidOperationException)
            {
                Console.WriteLine($"Error when deserialize '{Path.GetFileName(filePath)}'\n{e.Message}");
            }

            return Activator.CreateInstance<T>();
        }

        public static void Save(object obj, string filePath)
        {
            JsonSerializerOptions options = new()
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(obj, options);

            using (FileStream fileStream = new(filePath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                using (StreamWriter streamWriter = new(fileStream, Encoding.UTF8))
                    streamWriter.Write(json);
            }
        }

        public static JsonNode Diff(this JsonNode current, JsonNode model)
        {
            JsonNode result = JsonNode.Parse("{}")!;

            if (!current.ToString().Equals(model.ToString()))
            {
                if (current is JsonObject currentObject && model is JsonObject modelObject)
                {
                    IEnumerable<string> removedKeys = modelObject.Select(x => x.Key).Except(currentObject.Select(x => x.Key));
                    foreach (string key in removedKeys)
                    {
                        result[key] = new JsonObject
                        {
                            ["-"] = model[key]!.Deserialize<JsonNode>()
                        };
                    }

                    IEnumerable<string> addedKeys = currentObject.Select(x => x.Key).Except(modelObject.Select(x => x.Key));
                    foreach (string key in addedKeys)
                    {
                        result[key] = new JsonObject
                        {
                            ["+"] = current[key]!.Deserialize<JsonNode>()
                        };
                    }

                    IEnumerable<string> unchangedKeys = currentObject.Where(x => x.Value is not null && x.Value.ToString().Equals(model[x.Key])).Select(x => x.Key);
                    IEnumerable<string> potentiallyModifiedKeys = currentObject.Select(c => c.Key).Except(addedKeys).Except(unchangedKeys);
                    foreach (string key in potentiallyModifiedKeys)
                    {
                        JsonNode resultChild = current[key]!.Diff(model[key]!);

                        if (resultChild.AsObject().Count > 0)
                            result[key] = resultChild;
                    }
                }
                else if (current is JsonArray currentArray && model is JsonArray modelArray)
                {
                    IEnumerable<JsonNode?>? removedValues = modelArray.Except(currentArray);
                    IEnumerable<JsonNode?>? addedValues = currentArray.Except(modelArray);

                    JsonArray tempArray = new();
                    foreach (JsonNode? node in removedValues)
                    {
                        if (node is not null)
                            tempArray.Add(node.Deserialize<JsonNode>());
                    }
                    result["-"] = tempArray;

                    tempArray = new();
                    foreach (JsonNode? node in addedValues)
                    {
                        if (node is not null)
                            tempArray.Add(node.Deserialize<JsonNode>());
                    }
                    result["+"] = tempArray;
                }
                else
                {
                    result["-"] = model.Deserialize<JsonNode>();
                    result["+"] = current.Deserialize<JsonNode>();
                }
            }

            return result;
        }

        public static string Diff(string current, string model)
        {
            JsonNode? currentNode = JsonNode.Parse(current);
            JsonNode? modelNode = JsonNode.Parse(model);

            if (currentNode is null || modelNode is null)
                return "";

            JsonNode resultNode = currentNode.Diff(modelNode);

            JsonSerializerOptions options = new()
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };
            string result = resultNode.ToJsonString(options);

            if (result.Equals("{}"))
                return "";

            return result;
        }

        public static void RemoveFieldsRecursively(this JsonNode value, params string[] fields)
        {
            if (value is JsonObject jsonObject)
            {
                HashSet<string> fieldsFound = new();
                foreach (KeyValuePair<string, JsonNode?> child in jsonObject)
                {
                    if (fields.Any(x => x.Equals(child.Key)))
                        fieldsFound.Add(child.Key);
                    else if (child.Value is JsonObject or JsonArray)
                        child.Value.RemoveFieldsRecursively(fields);
                }

                foreach (string field in fieldsFound)
                    jsonObject.Remove(field);
            }
            else if (value is JsonArray jsonArray)
            {
                foreach (JsonNode? child in jsonArray)
                {
                    if (child is JsonObject or JsonArray)
                        child.RemoveFieldsRecursively(fields);
                }
            }
        }
    }
}
