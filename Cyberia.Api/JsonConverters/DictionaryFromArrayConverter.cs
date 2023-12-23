using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

public sealed class DictionaryFromArrayConverter<TKey, TValue> : JsonConverter<IReadOnlyDictionary<TKey, TValue>>
    where TKey : notnull
{
    public override IReadOnlyDictionary<TKey, TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var elements = JsonSerializer.Deserialize<JsonElement[]>(ref reader, options) ?? [];

        return elements.ToDictionary(
            x => JsonSerializer.Deserialize<TKey>(x[0].GetRawText(), options)!,
            x => JsonSerializer.Deserialize<TValue>(x[1].GetRawText(), options)!);
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
