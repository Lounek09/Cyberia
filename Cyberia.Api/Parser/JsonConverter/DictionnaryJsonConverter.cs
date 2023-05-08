using System.Text.Json.Serialization;
using System.Text.Json;

namespace Cyberia.Api.Factories.JsonConverter
{
    public sealed class DictionaryJsonConverter<TKey, TValue> : JsonConverter<Dictionary<TKey, TValue>> where TKey : notnull
    {
        public override Dictionary<TKey, TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Dictionary<TKey, TValue> dict = new();
            JsonElement element = JsonSerializer.Deserialize<JsonElement>(ref reader, options);

            if (element.ValueKind is not JsonValueKind.Array)
                throw new JsonException("Invalid JSON format: expected an array.");

            foreach (JsonElement pairElement in element.EnumerateArray())
            {
                if (pairElement.ValueKind == JsonValueKind.Array && pairElement.GetArrayLength() == 2)
                {
                    TKey? key = JsonSerializer.Deserialize<TKey>(pairElement[0].GetRawText(), options);
                    TValue? value = JsonSerializer.Deserialize<TValue>(pairElement[1].GetRawText(), options);

                    if (key is not null && value is not null)
                        dict[key] = value;
                }
            }

            return dict;
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<TKey, TValue> values, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            foreach (KeyValuePair<TKey, TValue> pair in values)
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
