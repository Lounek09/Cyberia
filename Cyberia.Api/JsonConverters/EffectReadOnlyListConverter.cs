using Cyberia.Api.Factories;
using Cyberia.Api.Factories.Effects;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

/// <summary>
/// A specialized JSON converter for serializing and deserializing
/// <see cref="IReadOnlyList{T}"/> containing <see cref="IEffect"/> objects.
/// </summary>
/// <remarks>
/// - Expects a JSON string containing a list of compressed <see cref="IEffect"/> representation.<br />
/// - Parses this string into a structured <see cref="IReadOnlyList{IEffect}"/> using <see cref="EffectFactory.CreateMany"/>.
/// </remarks>
public sealed class EffectReadOnlyListConverter : JsonConverter<IReadOnlyList<IEffect>>
{
    public override IReadOnlyList<IEffect> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"Expected {JsonTokenType.String} but got {reader.TokenType}.");
        }

        return EffectFactory.CreateMany(reader.GetString() ?? string.Empty).AsReadOnly();
    }

    public override void Write(Utf8JsonWriter writer, IReadOnlyList<IEffect> values, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
