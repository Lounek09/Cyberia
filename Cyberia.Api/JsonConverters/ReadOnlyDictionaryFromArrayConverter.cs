using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

public sealed class ReadOnlyDictionaryFromArrayConverter<TKey, TValue> : JsonConverter<IReadOnlyDictionary<TKey, TValue>>
    where TKey : notnull
{
    public override IReadOnlyDictionary<TKey, TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var elements = JsonSerializer.Deserialize<JsonElement[]>(ref reader, options) ?? [];

        Dictionary<TKey, TValue> dictionary = [];

        foreach (var element in elements)
        {
            if (element.ValueKind != JsonValueKind.Array || element.GetArrayLength() != 2)
            {
                throw new JsonException();
            }

            var key = JsonSerializer.Deserialize<TKey>(element[0], options) ?? throw new JsonException();
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(
                    key,
                    JsonSerializer.Deserialize<TValue>(element[1], options) ?? throw new JsonException());
            }
        }

        return dictionary;
    }

    public override void Write(Utf8JsonWriter writer, IReadOnlyDictionary<TKey, TValue> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        foreach (var pair in value)
        {
            writer.WriteStartArray();

            JsonSerializer.Serialize(writer, pair.Key, options);
            JsonSerializer.Serialize(writer, pair.Value, options);

            writer.WriteEndArray();
        }

        writer.WriteEndArray();
    }
}
