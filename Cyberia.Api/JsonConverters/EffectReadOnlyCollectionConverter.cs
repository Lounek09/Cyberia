using Cyberia.Api.Factories;
using Cyberia.Api.Factories.Effects;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

/// <summary>
/// A specialized JSON converter for serializing and deserializing <see cref="EffectReadOnlyCollection"/>.
/// </summary>
/// <remarks>
/// - Expects a JSON string containing a list of compressed <see cref="Effect"/> representation.<br />
/// - Parses this string into an <see cref="EffectReadOnlyCollection"/> using <see cref="EffectFactory.CreateMany(ReadOnlySpan{char}"/>.
/// </remarks>
public sealed class EffectReadOnlyCollectionConverter : JsonConverter<EffectReadOnlyCollection>
{
    public override EffectReadOnlyCollection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"Expected {JsonTokenType.String} but got {reader.TokenType}.");
        }

        var compressedEffects = reader.GetString();
        if (string.IsNullOrEmpty(compressedEffects))
        {
            return EffectReadOnlyCollection.Empty;
        }

        return EffectFactory.CreateMany(compressedEffects).AsReadOnly();
    }

    public override void Write(Utf8JsonWriter writer, EffectReadOnlyCollection values, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
