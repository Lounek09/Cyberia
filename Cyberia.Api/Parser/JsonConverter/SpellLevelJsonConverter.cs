using Cyberia.Api.Data;
using Cyberia.Api.Factories;
using Cyberia.Api.Factories.Effects;
using Cyberia.Api.Managers;
using Cyberia.Api.Values;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Parser.JsonConverter
{
    public sealed class SpellLevelJsonConverter : JsonConverter<SpellLevelData>
    {
        public override SpellLevelData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is not JsonTokenType.StartArray)
            {
                throw new JsonException("Invalid JSON format: expected an array.");
            }

            JsonElement[]? elements = JsonSerializer.Deserialize<JsonElement[]>(ref reader, options);
            if (elements is null || elements.Length != 21)
            {
                throw new JsonException($"Invalid JSON format: expected an array of 21 values, but got a length of {elements?.Length}.");
            }

            JsonElement[] effects = JsonSerializer.Deserialize<JsonElement[]>(elements[0].GetRawText(), options) ?? [];
            JsonElement[] criticalEffects = JsonSerializer.Deserialize<JsonElement[]>(elements[1].GetRawText(), options) ?? [];
            List<EffectArea> effectAreas = EffectAreaManager.GetEffectAreas(elements[15].ToString()).ToList();

            List<IEffect> effectParse = EffectFactory.GetEffectsParseFromSpell(effects, effectAreas).ToList();
            effectAreas.RemoveRange(0, effectParse.Count);
            List<IEffect> criticalEffectParse = EffectFactory.GetEffectsParseFromSpell(criticalEffects, effectAreas).ToList();

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
}
