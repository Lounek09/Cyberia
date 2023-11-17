using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters
{
    public sealed class StringListConverter : JsonConverter<List<string>>
    {
        public override List<string> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is not JsonTokenType.StartArray)
            {
                throw new JsonException("Invalid JSON format: expected an array.");
            }

            JsonElement[]? elements = JsonSerializer.Deserialize<JsonElement[]>(ref reader, options) ?? throw new JsonException($"Invalid JSON format: unable to deserialize into an array of JsonElement.");
            return elements.Select(x => x.ToString()).ToList();
        }

        public override void Write(Utf8JsonWriter writer, List<string> values, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            foreach (string value in values)
            {
                if (double.TryParse(value, out double doubleValue))
                {
                    writer.WriteNumberValue(doubleValue);
                    continue;
                }

                if (int.TryParse(value, out int intValue))
                {
                    writer.WriteNumberValue(intValue);
                    continue;
                }

                writer.WriteStringValue(value);
            }

            writer.WriteEndArray();
        }
    }
}
