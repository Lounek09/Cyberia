using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

/// <summary>
/// A specialized JSON converter for serializing and deserializing <see cref="bool"/> from integers.
/// </summary>
public sealed class BoolFromIntConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.Number)
        {
            throw new JsonException($"Expected {JsonTokenType.Number} but got {reader.TokenType}.");
        }

        var value = reader.GetInt32();

        if (value == 0)
        {
            return false;
        }

        if (value == 1)
        {
            return true;
        }

        throw new JsonException($"Expected 0 or 1 but got {value}.");
    }

    public override void Write(Utf8JsonWriter writer, bool values, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(values ? 1 : 0);
    }
}
