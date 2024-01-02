using Cyberia.Api.Data.Spells;
using Cyberia.Api.Factories;
using Cyberia.Api.Managers;
using Cyberia.Api.Values;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.JsonConverters;

public sealed class SpellLevelConverter : JsonConverter<SpellLevelData>
{
    public override SpellLevelData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var elements = JsonSerializer.Deserialize<JsonElement[]>(ref reader, options) ?? [];

        var effects = JsonSerializer.Deserialize<JsonElement[]>(elements[0].GetRawText(), options) ?? [];
        var criticalEffects = JsonSerializer.Deserialize<JsonElement[]>(elements[1].GetRawText(), options) ?? [];
        var effectAreas = EffectAreaManager.GetEffectAreas(elements[15].ToString()).ToList();
        var effectParse = EffectFactory.GetEffectsParseFromSpell(effects, effectAreas).ToList();
        effectAreas.RemoveRange(0, effectParse.Count);
        var criticalEffectParse = EffectFactory.GetEffectsParseFromSpell(criticalEffects, effectAreas).ToList();

        return new SpellLevelData
        {
            Effects = effectParse,
            CriticalEffects = criticalEffectParse,
            ActionPointCost = elements[2].GetInt32(),
            MinRange = elements[3].GetInt32(),
            MaxRange = elements[4].GetInt32(),
            CriticalHitRate = elements[5].GetInt32(),
            CriticalFailureRate = elements[6].GetInt32(),
            LineOnly = elements[7].GetBoolean(),
            LineOfSight = elements[8].GetBoolean(),
            NeedFreeCell = elements[9].GetBoolean(),
            CanBoostRange = elements[10].GetBoolean(),
            SpellLevelCategory = (SpellLevelCategory)elements[11].GetInt32(),
            LaunchCountByTurn = elements[12].GetInt32(),
            LaunchCountByPlayerByTurn = elements[13].GetInt32(),
            DelayBetweenLaunch = elements[14].GetInt32(),
            RequiredStatesId = JsonSerializer.Deserialize<List<int>>(elements[16].GetRawText(), options) ?? [],
            ForbiddenStatesId = JsonSerializer.Deserialize<List<int>>(elements[17].GetRawText(), options) ?? [],
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
