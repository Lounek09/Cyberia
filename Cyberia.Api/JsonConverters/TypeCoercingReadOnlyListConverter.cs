using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

/// <summary>
/// A specialized JSON converter for serializing and deserializing collections to <see cref="IReadOnlyList{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the list to convert to.</typeparam>
/// <remarks>
/// When reading JSON, this converter:
/// <list type="bullet">
///   <item>Expects a JSON array</item>
///   <item>For string types, performs special type coercion from various JSON types (numbers, booleans, etc.) to strings</item>
///   <item>For non-string types, uses the standard JSON deserialization process</item>
/// </list>
/// 
/// When writing JSON, it:
/// <list type="bullet">
///   <item>Serializes the collection as a JSON array</item>
/// </list>
/// </remarks>
public sealed class TypeCoercingReadOnlyListConverter<T> : JsonConverter<IReadOnlyList<T>>
{
    public override IReadOnlyList<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException($"Expected {JsonTokenType.StartArray} but got {reader.TokenType}");
        }

        if (typeof(T) == typeof(string))
        {
            List<string> values = [];

            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                values.Add(reader.TokenType switch
                {
                    JsonTokenType.String => reader.GetString() ?? string.Empty,
                    JsonTokenType.Number => reader.GetDouble().ToString(),
                    JsonTokenType.True => "true",
                    JsonTokenType.False => "false",
                    JsonTokenType.Null => string.Empty,
                    _ => throw new JsonException($"Unexpected token {reader.TokenType} while reading string.")
                });
            }

            return (IReadOnlyList<T>)values.AsReadOnly();
        }

        return JsonSerializer.Deserialize<IReadOnlyList<T>>(ref reader, options) ?? [];
    }

    public override void Write(Utf8JsonWriter writer, IReadOnlyList<T> values, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, values, options);
    }
}
