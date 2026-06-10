using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

/// <summary>
/// A specialized JSON converter for serializing and deserializing <see cref="LocalizedString"/> objects.
/// </summary>
/// <remarks>
/// - Expects a JSON string or number.<br />
/// - Converts to a <see cref="LocalizedString"/> with the value as the default translation.
/// </remarks>
public sealed class LocalizedStringConverter : JsonConverter<LocalizedString>
{
    public override LocalizedString Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => new LocalizedString(reader.GetString() ?? string.Empty),
            JsonTokenType.Number => new LocalizedString(reader.GetDouble().ToString()),
            JsonTokenType.True => new LocalizedString("true"),
            JsonTokenType.False => new LocalizedString("false"),
            JsonTokenType.Null => new LocalizedString(string.Empty),
            _ => throw new JsonException("Unexpected token type for LocalizedString conversion.")
        };
    }

    public override void Write(Utf8JsonWriter writer, LocalizedString value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
