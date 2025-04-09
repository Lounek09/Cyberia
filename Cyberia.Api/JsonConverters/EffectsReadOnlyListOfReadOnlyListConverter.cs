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
/// When reading JSON, this converter:
/// <list type="bullet">
///   <item>Expects a JSON array of strings, where each string represents a compressed list of effects</item>
///   <item>Uses <see cref="EffectFactory.CreateMany"/> to parse each string into a list of <see cref="IEffect"/> objects</item>
///   <item>Aggregates these lists into a read-only list of read-only lists</item>
/// </list>
/// 
/// When writing JSON, it:
/// <list type="bullet">
///   <item>Converts each <see cref="IReadOnlyList{IEffect}"/> into its compressed string representation</item>
///   <item>Writes these strings as a JSON array</item>
/// </list>
/// </remarks>
public sealed class EffectsReadOnlyListOfReadOnlyListConverter : JsonConverter<IReadOnlyList<IReadOnlyList<IEffect>>>
{
    public override IReadOnlyList<IReadOnlyList<IEffect>> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException($"Expected {JsonTokenType.StartArray} but got {reader.TokenType}");
        }

        List<IReadOnlyList<IEffect>> effects = [];

        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException($"Expected {JsonTokenType.String} but got {reader.TokenType}");
            }

            effects.Add(EffectFactory.CreateMany(reader.GetString() ?? string.Empty));
        }

        return effects.AsReadOnly();
    }

    public override void Write(Utf8JsonWriter writer, IReadOnlyList<IReadOnlyList<IEffect>> values, JsonSerializerOptions options)
    {
        //TODO: Implement the write method to serialize IEffect to JSON.
        throw new NotImplementedException();
    }
}
