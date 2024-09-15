using Cyberia.Api.Data.Spells;
using Cyberia.Api.Factories;
using Cyberia.Api.Values;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

public sealed class SpellLevelConverter : JsonConverter<SpellLevelData>
{
    public override SpellLevelData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var elements = JsonSerializer.Deserialize<JsonElement[]>(ref reader, options) ?? [];

        if (elements.Length < 21)
        {
            throw new JsonException();
        }

        var effects = JsonSerializer.Deserialize<JsonElement[]>(elements[0], options) ?? [];
        var criticalEffects = JsonSerializer.Deserialize<JsonElement[]>(elements[1], options) ?? [];
        var effectAreas = EffectAreaFactory.CreateMany(elements[15].GetStringOrEmpty());

        var effectParse = EffectFactory.CreateMany(effects, effectAreas);
        effectAreas.RemoveRange(0, effectParse.Count);
        var criticalEffectParse = EffectFactory.CreateMany(criticalEffects, effectAreas);

        return new SpellLevelData
        {
            Effects = effectParse,
            CriticalEffects = criticalEffectParse,
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
            RequiredStatesId = JsonSerializer.Deserialize<List<int>>(elements[16], options) ?? [],
            ForbiddenStatesId = JsonSerializer.Deserialize<List<int>>(elements[17], options) ?? [],
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
