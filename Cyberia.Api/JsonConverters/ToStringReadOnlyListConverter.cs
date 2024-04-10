using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

public sealed class ToStringReadOnlyListConverter : JsonConverter<IReadOnlyList<string>>
{
    public override IReadOnlyList<string> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var elements = JsonSerializer.Deserialize<JsonElement[]>(ref reader, options) ?? [];

        return elements
            .Select(x => x.ToString())
            .ToList();
    }

    public override void Write(Utf8JsonWriter writer, IReadOnlyList<string> values, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        foreach (var value in values)
        {
            writer.WriteStringValue(value);
        }

        writer.WriteEndArray();
    }
}
