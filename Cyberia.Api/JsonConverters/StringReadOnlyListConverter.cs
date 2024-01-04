using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

public sealed class StringReadOnlyListConverter
    : JsonConverter<IReadOnlyList<string>>
{
    public override IReadOnlyList<string> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var elements = JsonSerializer.Deserialize<JsonElement[]>(ref reader, options) ?? [];
        return elements.Select(x => x.ToString()).ToList();
    }

    public override void Write(Utf8JsonWriter writer, IReadOnlyList<string> values, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        foreach (var value in values)
        {
            if (double.TryParse(value, out var doubleValue))
            {
                writer.WriteNumberValue(doubleValue);
                continue;
            }

            if (int.TryParse(value, out var intValue))
            {
                writer.WriteNumberValue(intValue);
                continue;
            }

            writer.WriteStringValue(value);
        }

        writer.WriteEndArray();
    }
}
