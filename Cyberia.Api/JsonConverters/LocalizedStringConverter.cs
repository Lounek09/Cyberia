using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

public sealed class LocalizedStringConverter : JsonConverter<LocalizedString>
{
    public override LocalizedString Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var element = JsonElement.ParseValue(ref reader);

        return element.ValueKind switch
        {
            JsonValueKind.String => new LocalizedString(element.GetString() ?? string.Empty),
            JsonValueKind.Number => new LocalizedString(element.GetDouble().ToString()),
            _ => throw new JsonException()
        };
    }

    public override void Write(Utf8JsonWriter writer, LocalizedString value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Default);
    }
}
