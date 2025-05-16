using Cyberia.Api.Factories;
using Cyberia.Api.Factories.Effects;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

/// <summary>
/// A specialized JSON converter for serializing and deserializing 
/// <see cref="IReadOnlyList{T}"/> of <see cref="IReadOnlyList{T}"/> containing <see cref="IEffect"/> objects.
/// </summary>
/// <remarks>
/// - Expects a JSON array of strings, where each string represents a compressed list of effects<br />
/// - Uses <see cref="EffectFactory.CreateMany"/> to parse each string into a list of <see cref="IEffect"/> objects<br />
/// - Aggregates these lists into a read-only list of read-only lists.
/// </remarks>
public sealed class EffectReadOnlyListOfReadOnlyListConverter : JsonConverter<IReadOnlyList<IReadOnlyList<IEffect>>>
{
    private readonly EffectReadOnlyListConverter _effectReadOnlyListConverter = new();

    public override IReadOnlyList<IReadOnlyList<IEffect>> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException($"Expected {JsonTokenType.StartArray} but got {reader.TokenType}.");
        }

        List<IReadOnlyList<IEffect>> effects = [];

        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            effects.Add(_effectReadOnlyListConverter.Read(ref reader, typeof(IReadOnlyList<IEffect>), options));
        }

        return effects.AsReadOnly();
    }

    public override void Write(Utf8JsonWriter writer, IReadOnlyList<IReadOnlyList<IEffect>> values, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
