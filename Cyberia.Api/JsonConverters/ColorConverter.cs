using System.Drawing;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

/// <summary>
/// A specialized JSON converter for serializing and deserializing <see cref="Color"/> values from decimal or hexadecimal RGB representations.
/// </summary>
/// <remarks>
/// - Expects a JSON number or string containing a decimal or hexadecimal RGB representation.<br />
/// - Parses this number or string into a structured <see cref="Color"/> using bitwise operations.
/// </remarks>
public sealed class ColorConverter : JsonConverter<Color?>
{
    public override Color? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.TokenType switch
        {
            JsonTokenType.Number => ReadNumberToken(ref reader),
            JsonTokenType.String => ReadStringToken(ref reader),
            _ => throw new JsonException($"Expected {JsonTokenType.Number} or {JsonTokenType.String} but got {reader.TokenType}.")
        };

        if (value == -1)
        {
            return null;
        }

        var r = (value >> 16) & 0xFF;
        var g = (value >> 8) & 0xFF;
        var b = value & 0xFF;

        return Color.FromArgb(r, g, b);
    }

    public override void Write(Utf8JsonWriter writer, Color? values, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    private static int ReadNumberToken(ref Utf8JsonReader reader)
    {
        if (!reader.TryGetInt32(out var value))
        {
            throw new JsonException($"Expected an integer but got {reader.GetString()}.");
        }

        return value;
    }

    private static int ReadStringToken(ref Utf8JsonReader reader)
    {
        var valueString = reader.GetString();
        if (string.IsNullOrEmpty(valueString))
        {
            throw new JsonException($"Expected a string representing a color but got null or empty.");
        }

        ReadOnlySpan<char> valueSpan = valueString.AsSpan();
        if (valueSpan[0] == '#')
        {
            valueSpan = valueSpan[1..];
        }
        else if (valueSpan.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        {
            valueSpan = valueSpan[2..];
        }

        if (!int.TryParse(valueSpan, out var value) &&
            !int.TryParse(valueSpan, NumberStyles.HexNumber, null, out value))
        {
            throw new JsonException($"Expected a hexadecimal string but got {valueString}.");
        }

        return value;
    }
}
