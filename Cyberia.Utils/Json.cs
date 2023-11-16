using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Cyberia.Utils
{
    public static class Json
    {
        private static readonly JsonSerializerOptions _indentOptions = new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        };

        public static T Load<T>(string json) where T : new()
        {
            try
            {
                return JsonSerializer.Deserialize<T>(json) ?? new();
            }
            catch (Exception e) when (e is JsonException or NotSupportedException or InvalidOperationException)
            {
                Log.Error(e, "Failed to deserialize the JSON");
            }

            return new();
        }

        public static T LoadFromFile<T>(string filePath) where T : new()
        {
            try
            {
                string json = File.ReadAllText(filePath);

                return Load<T>(json);
            }
            catch (Exception e) when (e is FileNotFoundException or DirectoryNotFoundException)
            {
                Log.Error(e, "File {FilePath} not found", filePath);
            }

            return new();
        }

        public static void Save(object obj, string filePath, JsonSerializerOptions? options = null)
        {
            string json = JsonSerializer.Serialize(obj, options);

            using FileStream fileStream = new(filePath, FileMode.Create, FileAccess.Write, FileShare.Write);
            using StreamWriter streamWriter = new(fileStream, Encoding.UTF8);

            streamWriter.Write(json);
        }

        public static JsonNode Diff(this JsonNode current, JsonNode model)
        {
            JsonObject result = new();

            if (JsonNode.DeepEquals(current, model))
            {
                return result;
            }

            if (current is JsonObject currentObject && model is JsonObject modelObject)
            {
                IEnumerable<string> removedKeys = modelObject.Select(x => x.Key).Except(currentObject.Select(x => x.Key));
                foreach (string key in removedKeys)
                {
                    result[key] = new JsonObject
                    {
                        ["-"] = model[key]!.DeepClone()
                    };
                }

                IEnumerable<string> addedKeys = currentObject.Select(x => x.Key).Except(modelObject.Select(x => x.Key));
                foreach (string key in addedKeys)
                {
                    result[key] = new JsonObject
                    {
                        ["+"] = current[key]!.DeepClone()
                    };
                }

                IEnumerable<string> unchangedKeys = currentObject.Where(x => JsonNode.DeepEquals(x.Value, model[x.Key])).Select(x => x.Key);
                IEnumerable<string> potentiallyModifiedKeys = currentObject.Select(x => x.Key).Except(addedKeys).Except(unchangedKeys);
                foreach (string key in potentiallyModifiedKeys)
                {
                    JsonNode resultChild = current[key]!.Diff(model[key]!);

                    if (resultChild.AsObject().Count > 0)
                    {
                        result[key] = resultChild;
                    }
                }
            }
            else if (current is JsonArray currentArray && model is JsonArray modelArray)
            {
                IEnumerable<JsonNode?> removedValues = modelArray.Except(currentArray);
                IEnumerable<JsonNode?> addedValues = currentArray.Except(modelArray);

                JsonArray tempArray = [];
                foreach (JsonNode? node in removedValues)
                {
                    if (node is not null)
                    {
                        tempArray.Add(node.DeepClone());
                    }
                }
                result["-"] = tempArray;

                tempArray = [];
                foreach (JsonNode? node in addedValues)
                {
                    if (node is not null)
                    {
                        tempArray.Add(node.DeepClone());
                    }
                }
                result["+"] = tempArray;
            }
            else
            {
                result["-"] = model.DeepClone();
                result["+"] = current.DeepClone();
            }

            return result;
        }

        public static string Diff(string current, string model)
        {
            JsonNode? currentNode = JsonNode.Parse(current);
            JsonNode? modelNode = JsonNode.Parse(model);

            if (currentNode is null || modelNode is null)
            {
                return string.Empty;
            }

            JsonNode resultNode = currentNode.Diff(modelNode);

            string result = resultNode.ToJsonString(_indentOptions);

            if (result.Equals("{}"))
            {
                return string.Empty;
            }

            return result;
        }
    }
}
