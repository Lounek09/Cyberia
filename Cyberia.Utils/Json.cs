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
            if (current.Equals(model))
            {
                return string.Empty;
            }

            JsonNode? currentNode;
            JsonNode? modelNode;
            try
            {
                currentNode = JsonNode.Parse(current);
                modelNode = JsonNode.Parse(model);
            }
            catch (JsonException e)
            {
                Log.Error(e, "Failed to parse the given JSON to diff");
                return string.Empty;
            }

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
