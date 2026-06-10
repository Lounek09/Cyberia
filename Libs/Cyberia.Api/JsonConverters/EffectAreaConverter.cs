using Cyberia.Api.Factories;
using Cyberia.Api.Factories.EffectAreas;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

/// <summary>
/// A specialized JSON converter for serializing and deserializing <see cref="EffectArea"/> objects.
/// </summary>
/// <remarks>
/// - Expects a JSON string containing a compressed <see cref="EffectArea"/> representation.<br />
/// - Parses this string into a structured <see cref="EffectArea"/> using <see cref="EffectAreaFactory.Create"/>.
/// </remarks>
public sealed class EffectAreaConverter : JsonConverter<EffectArea>
{
    public override EffectArea Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"Expected {JsonTokenType.String} but got {reader.TokenType}.");
        }

        return EffectAreaFactory.Create(reader.GetString() ?? string.Empty);
    }

    public override void Write(Utf8JsonWriter writer, EffectArea value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
