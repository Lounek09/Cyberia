using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

/// <summary>
/// A JSON converter for deserializing arrays of <see cref="Color"/> values from decimal or hexadecimal RGB representations.
/// </summary>
/// <remarks>
/// - Expects a JSON array containing numbers or strings of decimal or hexadecimal RGB representations.<br />
/// - Parses these numbers or strings into a structured <see cref="IReadOnlyList{Color?}"/> using bitwise operations.
/// </remarks>
public sealed class ColorReadOnlyListConverter : JsonConverter<IReadOnlyList<Color?>>
{
    private static readonly ColorConverter _colorConverter = new();

    public override IReadOnlyList<Color?> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException($"Expected {JsonTokenType.StartArray} but got {reader.TokenType}.");
        }

        List<Color?> colors = [];

        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            colors.Add(_colorConverter.Read(ref reader, typeof(Color?), options));
        }

        return colors.AsReadOnly();
    }

    public override void Write(Utf8JsonWriter writer, IReadOnlyList<Color?> values, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
