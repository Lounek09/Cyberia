using Cyberia.Api.Data.Spells;
using Cyberia.Api.Enums;
using Cyberia.Api.Factories;

using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

/// <summary>
/// A specialized JSON converter for serializing and deserializing <see cref="SpellLevelData"/> objects.
/// </summary>
/// <remarks>
/// - Expects a JSON array with 21 elements representing the properties of <see cref="SpellLevelData"/>.<br />
/// - Parses effects and critical effects using <see cref="EffectFactory.CreateMany"/> and effect areas using <see cref="EffectAreaFactory.CreateMany"/>.
/// </remarks>
public sealed class SpellLevelConverter : JsonConverter<SpellLevelData>
{
    public override SpellLevelData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException($"Expected {JsonTokenType.StartArray} but got {reader.TokenType}.");
        }

        var elements = JsonElement.ParseValue(ref reader);

        var effectAreas = CollectionsMarshal.AsSpan(EffectAreaFactory.CreateMany(elements[15].GetString()));

        var parsedEffects = EffectFactory.CreateMany(elements[0], effectAreas);
        var parsedCriticalEffects = EffectFactory.CreateMany(elements[1], effectAreas[parsedEffects.Count..]);

        return new SpellLevelData
        {
            Effects = parsedEffects,
            CriticalEffects = parsedCriticalEffects,
            ActionPointCost = elements[2].GetInt32(),
            MinRange = elements[3].GetInt32(),
            MaxRange = elements[4].GetInt32(),
            CriticalHitRate = elements[5].GetInt32(),
            CriticalFailureRate = elements[6].GetInt32(),
            Linear = elements[7].GetBoolean(),
            LineOfSight = elements[8].GetBoolean(),
            NeedFreeCell = elements[9].GetBoolean(),
            AdjustableRange = elements[10].GetBoolean(),
            SpellLevelCategory = (SpellLevelCategory)elements[11].GetInt32(),
            CastPerTurn = elements[12].GetInt32(),
            CastPerPlayer = elements[13].GetInt32(),
            TurnsBetweenCast = elements[14].GetInt32(),
            RequiredStatesId = JsonSerializer.Deserialize<IReadOnlyList<int>>(elements[16], options) ?? [],
            ForbiddenStatesId = JsonSerializer.Deserialize<IReadOnlyList<int>>(elements[17], options) ?? [],
            NeededLevel = elements[18].GetInt32(),
            CricalFailureEndTheTurn = elements[19].GetBoolean(),
            Id = elements[20].GetInt32()
        };
    }

    public override void Write(Utf8JsonWriter writer, SpellLevelData value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
