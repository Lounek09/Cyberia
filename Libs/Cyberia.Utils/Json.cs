using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Cyberia.Utils;

/// <summary>
/// Provides utility methods for working with JSON.
/// </summary>
// TODO: Rework, output not consistent between the two diff methods
public static class Json
{
    private static readonly JsonSerializerOptions s_indentOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true
    };

    /// <summary>
    /// Computes the difference between two JSON strings.
    /// </summary>
    /// <param name="current">The current JSON string.</param>
    /// <param name="model">The model JSON string to compare with.</param>
    /// <returns>A string representing the difference between the current and model JSON strings.</returns>
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

        var resultNode = Diff(currentNode, modelNode);
        var result = resultNode.ToJsonString(s_indentOptions);

        if (result.Equals("{}"))
        {
            return string.Empty;
        }

        return result;
    }

    /// <summary>
    /// Computes the difference between two JSON nodes.
    /// </summary>
    /// <param name="current">The current JSON node.</param>
    /// <param name="model">The model JSON node to compare with.</param>
    /// <returns>A JSON node representing the difference between the current and model nodes.</returns>
    public static JsonNode Diff(JsonNode? current, JsonNode? model)
    {
        JsonObject result = [];

        var isCurrentNull = current is null;
        var isModelNull = model is null;

        if (isCurrentNull && isModelNull)
        {
            return result;
        }

        if (isCurrentNull && !isModelNull)
        {
            result["-"] = model!.DeepClone();
            return result;
        }

        if (!isCurrentNull && isModelNull)
        {
            result["+"] = current!.DeepClone();
            return result;
        }

        if (JsonNode.DeepEquals(current, model))
        {
            return result;
        }

        if (current is JsonObject currentObject && model is JsonObject modelObject)
        {
            var removedKeys = modelObject.Select(x => x.Key).Except(currentObject.Select(x => x.Key));
            foreach (var key in removedKeys)
            {
                result[key] = new JsonObject
                {
                    ["-"] = model[key]!.DeepClone()
                };
            }

            var addedKeys = currentObject.Select(x => x.Key).Except(modelObject.Select(x => x.Key));
            foreach (var key in addedKeys)
            {
                result[key] = new JsonObject
                {
                    ["+"] = current[key]!.DeepClone()
                };
            }

            var unchangedKeys = currentObject.Where(x => JsonNode.DeepEquals(x.Value, model[x.Key])).Select(x => x.Key);
            var potentiallyModifiedKeys = currentObject.Select(x => x.Key).Except(addedKeys).Except(unchangedKeys);
            foreach (var key in potentiallyModifiedKeys)
            {
                var resultChild = Diff(current[key], model[key]);

                if (resultChild.AsObject().Count > 0)
                {
                    result[key] = resultChild;
                }
            }
        }
        else if (current is JsonArray currentArray && model is JsonArray modelArray)
        {
            var removedValues = modelArray.Except(currentArray);
            var addedValues = currentArray.Except(modelArray);

            JsonArray tempArray = [];
            foreach (var node in removedValues)
            {
                if (node is not null)
                {
                    tempArray.Add(node.DeepClone());
                }
            }
            result["-"] = tempArray;

            tempArray = [];
            foreach (var node in addedValues)
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
            result["-"] = model!.DeepClone();
            result["+"] = current!.DeepClone();
        }

        return result;
    }
}
