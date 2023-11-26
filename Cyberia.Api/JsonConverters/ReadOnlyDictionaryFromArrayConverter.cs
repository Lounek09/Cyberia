using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters
{
    public sealed class ReadOnlyDictionaryFromArrayConverter<TKey, TValue> : JsonConverter<ReadOnlyDictionary<TKey, TValue>>
        where TKey : notnull
    {
        public override ReadOnlyDictionary<TKey, TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonElement[] elements = JsonSerializer.Deserialize<JsonElement[]>(ref reader, options) ?? [];

            return elements.ToDictionary(
                x => JsonSerializer.Deserialize<TKey>(x[0].GetRawText(), options)!,
                x => JsonSerializer.Deserialize<TValue>(x[1].GetRawText(), options)!)
                .AsReadOnly();
        }

        public override void Write(Utf8JsonWriter writer, ReadOnlyDictionary<TKey, TValue> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            foreach (KeyValuePair<TKey, TValue> pair in value)
            {
                writer.WriteStartArray();

                JsonSerializer.Serialize(writer, pair.Key, options);
                JsonSerializer.Serialize(writer, pair.Value, options);

                writer.WriteEndArray();
            }

            writer.WriteEndArray();
        }
    }
}
