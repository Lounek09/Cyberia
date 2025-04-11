using Cyberia.Api.Factories;
using Cyberia.Api.Factories.Effects;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

/// <summary>
/// A specialized JSON converter for serializing and deserializing <see cref="IEffect"/> objects.
/// </summary>
/// <remarks>
/// - Expects a JSON string containing a compressed <see cref="IEffect"/> representation.<br />
/// - Parses this string into a structured <see cref="IEffect"/> using <see cref="EffectFactory.Create"/>.
/// </remarks>
public sealed class EffectConverter : JsonConverter<IEffect>
{
    public override IEffect Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"Expected {JsonTokenType.String} but got {reader.TokenType}.");
        }

        return EffectFactory.Create(reader.GetString() ?? string.Empty);
    }

    public override void Write(Utf8JsonWriter writer, IEffect values, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
