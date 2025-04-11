using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

/// <summary>
/// A specialized JSON converter for serializing and deserializing <see cref="IReadOnlyDictionary{TKey, TValue}"/> objects.
/// </summary>
/// <typeparam name="TKey">The type of the dictionary keys. Must be non-nullable.</typeparam>
/// <typeparam name="TValue">The type of the dictionary values.</typeparam>
/// <remarks>
/// - Expects a JSON array where each element is a key-value pair represented as a JSON array with two elements<br />
/// </remarks>
public sealed class ReadOnlyDictionaryFromArrayConverter<TKey, TValue> : JsonConverter<IReadOnlyDictionary<TKey, TValue>>
    where TKey : notnull
{
    public override IReadOnlyDictionary<TKey, TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException($"Expected {JsonTokenType.StartArray} but got {reader.TokenType}");
        }

        Dictionary<TKey, TValue> dictionary = [];
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException($"Expected {JsonTokenType.StartArray} but got {reader.TokenType}.");
            }

            reader.Read();
            var key = JsonSerializer.Deserialize<TKey>(ref reader, options)
                ?? throw new JsonException($"Failed to deserialize key of type {typeof(TKey)}.");

            reader.Read();
            var value = JsonSerializer.Deserialize<TValue>(ref reader, options)
                ?? throw new JsonException($"Failed to deserialize value of type {typeof(TValue)}.");

            if (!dictionary.TryAdd(key, value))
            {
                Log.Warning("Duplicate key {Key} found in JSON array. Skipping this entry.", key);
            }

            if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray)
            {
                throw new JsonException($"Expected {JsonTokenType.EndArray} but got {reader.TokenType}.");
            }
        }

        return dictionary.AsReadOnly();
    }

    public override void Write(Utf8JsonWriter writer, IReadOnlyDictionary<TKey, TValue> values, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
