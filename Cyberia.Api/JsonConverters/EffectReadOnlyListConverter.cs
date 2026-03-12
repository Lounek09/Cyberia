using Cyberia.Api.Factories;
using Cyberia.Api.Factories.Effects;

using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

/// <summary>
/// A specialized JSON converter for serializing and deserializing
/// <see cref="IReadOnlyList{T}"/> containing <see cref="Effect"/> objects.
/// </summary>
/// <remarks>
/// - Expects a JSON string containing a list of compressed <see cref="Effect"/> representation.<br />
/// - Parses this string into a structured <see cref="IReadOnlyList{T}"/> of <see cref="Effect"/> using <see cref="EffectFactory.CreateMany"/>.
/// </remarks>
public sealed class EffectReadOnlyListConverter : JsonConverter<IReadOnlyList<Effect>>
{
    public override IReadOnlyList<Effect> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"Expected {JsonTokenType.String} but got {reader.TokenType}.");
        }

        var compressedEffects = reader.GetString();
        if (string.IsNullOrEmpty(compressedEffects))
        {
            return ReadOnlyCollection<Effect>.Empty;
        }

        return EffectFactory.CreateMany(compressedEffects).AsReadOnly();
    }

    public override void Write(Utf8JsonWriter writer, IReadOnlyList<Effect> values, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
