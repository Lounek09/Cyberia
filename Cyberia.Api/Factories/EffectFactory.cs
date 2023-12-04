using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects;
using Cyberia.Api.Managers;

using System.Globalization;
using System.Text.Json;

namespace Cyberia.Api.Factories;

public readonly record struct EffectParameters(int Param1, int Param2, int Param3, string Param4);

public static class EffectFactory
{
    private static readonly Dictionary<int, Func<int, EffectParameters, int, int, CriteriaCollection, EffectArea, IEffect>> _factory = new()
    {
        { -1, PaddockItemEffectivenessEffect.Create },
        { 10, LearnEmoteEffect.Create },
        { 34, QuestEffect.Create },
        { 35, QuestEffect.Create },
        { 77, MinMaxEffect.Create },
        { 78, MinMaxEffect.Create },
        { 81, MinMaxEffect.Create },
        { 82, MinMaxEffect.Create },
        { 84, MinMaxEffect.Create },
        { 85, MinMaxEffect.Create },
        { 86, MinMaxEffect.Create },
        { 87, MinMaxEffect.Create },
        { 88, MinMaxEffect.Create },
        { 89, MinMaxEffect.Create },
        { 90, MinMaxEffect.Create },
        { 91, MinMaxEffect.Create },
        { 92, MinMaxEffect.Create },
        { 93, MinMaxEffect.Create },
        { 94, MinMaxEffect.Create },
        { 95, MinMaxEffect.Create },
        { 96, MinMaxEffect.Create },
        { 97, MinMaxEffect.Create },
        { 98, MinMaxEffect.Create },
        { 99, MinMaxEffect.Create },
        { 100, MinMaxEffect.Create },
        { 101, MinMaxEffect.Create },
        { 105, MinMaxEffect.Create },
        { 107, MinMaxEffect.Create },
        { 108, MinMaxEffect.Create },
        { 109, MinMaxEffect.Create },
        { 110, MinMaxEffect.Create },
        { 111, MinMaxEffect.Create },
        { 112, MinMaxEffect.Create },
        { 113, MinMaxEffect.Create },
        { 115, MinMaxEffect.Create },
        { 116, MinMaxEffect.Create },
        { 117, MinMaxEffect.Create },
        { 118, MinMaxEffect.Create },
        { 119, MinMaxEffect.Create },
        { 120, MinMaxEffect.Create },
        { 121, MinMaxEffect.Create },
        { 122, MinMaxEffect.Create },
        { 123, MinMaxEffect.Create },
        { 124, MinMaxEffect.Create },
        { 125, MinMaxEffect.Create },
        { 126, MinMaxEffect.Create },
        { 127, MinMaxEffect.Create },
        { 128, MinMaxEffect.Create },
        { 130, MinMaxEffect.Create },
        { 133, MinMaxEffect.Create },
        { 134, MinMaxEffect.Create },
        { 135, MinMaxEffect.Create },
        { 136, MinMaxEffect.Create },
        { 137, MinMaxEffect.Create },
        { 138, MinMaxEffect.Create },
        { 139, MinMaxEffect.Create },
        { 142, MinMaxEffect.Create },
        { 143, MinMaxEffect.Create },
        { 144, MinMaxEffect.Create },
        { 145, MinMaxEffect.Create },
        { 152, MinMaxEffect.Create },
        { 153, MinMaxEffect.Create },
        { 154, MinMaxEffect.Create },
        { 155, MinMaxEffect.Create },
        { 156, MinMaxEffect.Create },
        { 157, MinMaxEffect.Create },
        { 158, MinMaxEffect.Create },
        { 159, MinMaxEffect.Create },
        { 160, MinMaxEffect.Create },
        { 161, MinMaxEffect.Create },
        { 162, MinMaxEffect.Create },
        { 163, MinMaxEffect.Create },
        { 165, IncreaseWeaponDamageEffect.Create },
        { 166, MinMaxEffect.Create },
        { 168, MinMaxEffect.Create },
        { 169, MinMaxEffect.Create },
        { 171, MinMaxEffect.Create },
        { 172, MinMaxEffect.Create },
        { 173, MinMaxEffect.Create },
        { 174, MinMaxEffect.Create },
        { 175, MinMaxEffect.Create },
        { 176, MinMaxEffect.Create },
        { 177, MinMaxEffect.Create },
        { 178, MinMaxEffect.Create },
        { 179, MinMaxEffect.Create },
        { 181, SummonMonsterInFightEffect.Create },
        { 182, MinMaxEffect.Create },
        { 183, MinMaxEffect.Create },
        { 184, MinMaxEffect.Create },
        { 185, SummonStaticMonsterInFightEffect.Create },
        { 186, MinMaxEffect.Create },
        { 192, DeleteItemEffect.Create },
        { 193, GiveItemEffect.Create },
        { 194, MinMaxEffect.Create },
        { 197, TransformIntoMonsterEffect.Create },
        { 208, LaunchSpellGfxAnimationEffect.Create },
        { 210, MinMaxEffect.Create },
        { 211, MinMaxEffect.Create },
        { 212, MinMaxEffect.Create },
        { 213, MinMaxEffect.Create },
        { 214, MinMaxEffect.Create },
        { 215, MinMaxEffect.Create },
        { 216, MinMaxEffect.Create },
        { 217, MinMaxEffect.Create },
        { 218, MinMaxEffect.Create },
        { 219, MinMaxEffect.Create },
        { 220, MinMaxEffect.Create },
        { 221, GiveItemEffect.Create },
        { 225, MinMaxEffect.Create },
        { 226, MinMaxEffect.Create },
        { 228, LaunchSpellGfxAnimationEffect.Create },
        { 229, GiveRideEffect.Create },
        { 233, RemoveItemAroundEffect.Create },
        { 239, TransformIntoMonsterEffect.Create },
        { 240, MinMaxEffect.Create },
        { 241, MinMaxEffect.Create },
        { 242, MinMaxEffect.Create },
        { 243, MinMaxEffect.Create },
        { 244, MinMaxEffect.Create },
        { 245, MinMaxEffect.Create },
        { 246, MinMaxEffect.Create },
        { 247, MinMaxEffect.Create },
        { 248, MinMaxEffect.Create },
        { 249, MinMaxEffect.Create },
        { 250, MinMaxEffect.Create },
        { 251, MinMaxEffect.Create },
        { 252, MinMaxEffect.Create },
        { 253, MinMaxEffect.Create },
        { 254, MinMaxEffect.Create },
        { 255, MinMaxEffect.Create },
        { 256, MinMaxEffect.Create },
        { 257, MinMaxEffect.Create },
        { 258, MinMaxEffect.Create },
        { 259, MinMaxEffect.Create },
        { 260, MinMaxEffect.Create },
        { 261, MinMaxEffect.Create },
        { 262, MinMaxEffect.Create },
        { 263, MinMaxEffect.Create },
        { 264, MinMaxEffect.Create },
        { 265, MinMaxEffect.Create },
        { 266, MinMaxEffect.Create },
        { 267, MinMaxEffect.Create },
        { 268, MinMaxEffect.Create },
        { 269, MinMaxEffect.Create },
        { 270, MinMaxEffect.Create },
        { 271, MinMaxEffect.Create },
        { 275, MinMaxEffect.Create },
        { 276, MinMaxEffect.Create },
        { 277, MinMaxEffect.Create },
        { 278, MinMaxEffect.Create },
        { 279, MinMaxEffect.Create },
        { 281, ModifySpellEffect.Create },
        { 282, ModifySpellEffect.Create },
        { 283, ModifySpellEffect.Create },
        { 284, ModifySpellEffect.Create },
        { 285, ModifySpellEffect.Create },
        { 286, ModifySpellEffect.Create },
        { 287, ModifySpellEffect.Create },
        { 288, ModifySpellEffect.Create },
        { 289, ModifySpellEffect.Create },
        { 290, ModifySpellEffect.Create },
        { 291, ModifySpellEffect.Create },
        { 292, ModifySpellEffect.Create },
        { 293, ModifySpellEffect.Create },
        { 294, ModifySpellEffect.Create },
        { 300, LaunchSpellLevelEffect.Create },
        { 320, MinMaxEffect.Create },
        { 400, TrapSpellEffect.Create },
        { 401, GlyphSpellEffect.Create },
        { 402, GlyphSpellEffect.Create },
        { 405, KIllAndSummonEffect.Create },
        { 521, UnbreakableEffect.Create },
        { 601, TeleportToMapEffect.Create },
        { 604, LearnSpellLevelEffect.Create },
        { 605, MinMaxEffect.Create },
        { 606, MinMaxEffect.Create },
        { 607, MinMaxEffect.Create },
        { 608, MinMaxEffect.Create },
        { 609, MinMaxEffect.Create },
        { 610, MinMaxEffect.Create },
        { 611, MinMaxEffect.Create },
        { 612, MinMaxEffect.Create },
        { 613, MinMaxEffect.Create },
        { 614, GiveJobXpEffect.Create },
        { 615, ForgetJobEffect.Create },
        { 616, ForgetSpellEffect.Create },
        { 621, SummonMonsterEffect.Create },
        { 623, SummonMonsterFromSoulStoneEffect.Create },
        { 624, ForgetSpellEffect.Create },
        { 628, SummonMonsterFromSoulGemEffect.Create },
        { 646, MinMaxEffect.Create },
        { 649, AlignmentEffect.Create },
        { 670, MinMaxEffect.Create },
        { 671, MinMaxEffect.Create },
        { 672, MinMaxEffect.Create },
        { 699, LinkJobEffect.Create },
        { 701, MinMaxEffect.Create },
        { 702, MinMaxEffect.Create },
        { 715, MonsterSuperRaceEffect.Create },
        { 716, MonsterRaceEffect.Create },
        { 717, MonsterKillCounterEffect.Create },
        { 724, DisplayTitleEffect.Create },
        { 750, MinMaxEffect.Create },
        { 751, MinMaxEffect.Create },
        { 770, MinMaxEffect.Create },
        { 771, MinMaxEffect.Create },
        { 772, MinMaxEffect.Create },
        { 773, MinMaxEffect.Create },
        { 774, MinMaxEffect.Create },
        { 775, MinMaxEffect.Create },
        { 776, MinMaxEffect.Create },
        { 780, MinMaxEffect.Create },
        { 787, LaunchSpellEffect.Create },
        { 791, MinMaxEffect.Create },
        { 805, ReceivedOnDateTimeEffect.Create },
        { 806, PetCorpulenceEffect.Create },
        { 807, LastMealPetEffect.Create },
        { 808, LastMealDateTimeEffect.Create },
        { 814, KeyEffect.Create },
        { 830, GuildTeleportationEffect.Create },
        { 905, LaunchFightEffect.Create },
        { 939, EnhancePetEffect.Create },
        { 945, GiveRideAbilityEffect.Create },
        { 950, AddStateEffect.Create },
        { 951, RemoveStateEffect.Create },
        { 960, AlignmentEffect.Create },
        { 969, ItemLookEffect.Create },
        { 970, LivingItemEffect.Create },
        { 971, LivingItemCorpulenceEffect.Create },
        { 973, CompatibleWithItemTypeEffect.Create },
        { 983, ExchangeableEffect.Create },
        { 986, MinMaxEffect.Create },
        { 999, TeleportToMap2Effect.Create },
        { 2001, MinMaxEffect.Create },
        { 2008, MinMaxEffect.Create },
        { 2009, MinMaxEffect.Create },
        { 2010, MinMaxEffect.Create },
        { 2011, MinMaxEffect.Create },
        { 2100, MinMaxEffect.Create },
        { 2101, GiveTTGCardEffect.Create },
        { 2102, AddTTGCardToBinderEffect.Create },
        { 2112, MinMaxEffect.Create },
        { 2113, MinMaxEffect.Create },
        { 2114, MinMaxEffect.Create },
        { 2118, MinMaxEffect.Create },
        { 2126, MinMaxEffect.Create },
        { 2128, AddStateEffect.Create },
        { 2129, RemoveStateEffect.Create },
        { 2134, MinMaxEffect.Create },
        { 2135, MinMaxEffect.Create },
        { 2136, MinMaxEffect.Create },
        { 2137, AddStateEffect.Create },
        { 2138, ModifySpellEffect.Create },
        { 2143, TeleportMonsterEffect.Create },
        { 2144, SummonMonsterInFightEffect.Create },
        { 2150, DisplayEffectsFromItemEffect.Create }
    };

    public static IEffect GetEffect(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        if (_factory.TryGetValue(effectId, out var builder))
        {
            return builder(effectId, parameters, duration, probability, criteria, effectArea);
        }

        return UntranslatedEffect.Create(effectId, parameters, duration, probability, criteria, effectArea);
    }

    public static IEnumerable<IEffect> GetEffectsParseFromSpell(JsonElement[] effects, IReadOnlyList<EffectArea> effectAreas)
    {
        for (var i = 0; i < effects.Length; i++)
        {
            var effect = effects[i];

            var id = effect[0].GetInt32();
            var param1 = effect[1].ValueKind is JsonValueKind.Null ? 0 : effect[1].GetInt32();
            var param2 = effect[2].ValueKind is JsonValueKind.Null ? 0 : effect[2].GetInt32();
            var param3 = effect[3].ValueKind is JsonValueKind.Null ? 0 : effect[3].GetInt32();
            var param4 = effect.GetArrayLength() > 7 && effect[7].ValueKind is not JsonValueKind.Null ? effect[7].GetString() ?? string.Empty : string.Empty;
            var parameters = new EffectParameters(param1, param2, param3, param4);
            var duration = effect[4].ValueKind == JsonValueKind.Null ? 0 : effect[4].GetInt32();
            var probability = effect[5].ValueKind == JsonValueKind.Null ? 0 : effect[5].GetInt32();
            var criteria = CriterionFactory.GetCriteria(effect[6].GetString() ?? string.Empty);

            yield return GetEffect(id, parameters, duration, probability, criteria, effectAreas[i]);
        }
    }

    public static IEnumerable<IEffect> GetEffectsParseFromItem(string effects)
    {
        foreach (var effect in effects.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            var args = effect.Split("#");

            var id = args[0].StartsWith('-') ? int.Parse(args[0]) : int.Parse(args[0], NumberStyles.HexNumber);
            var param1 = args.Length > 1 && !string.IsNullOrEmpty(args[1]) ? args[1].StartsWith('-') ? int.Parse(args[1]) : int.Parse(args[1], NumberStyles.HexNumber) : 0;
            var param2 = args.Length > 2 && !string.IsNullOrEmpty(args[2]) ? args[1].StartsWith('-') ? int.Parse(args[2]) : int.Parse(args[2], NumberStyles.HexNumber) : 0;
            var param3 = args.Length > 3 && !string.IsNullOrEmpty(args[3]) ? args[1].StartsWith('-') ? int.Parse(args[3]) : int.Parse(args[3], NumberStyles.HexNumber) : 0;
            var param4 = args.Length > 4 ? args[4] : string.Empty;
            var parameters = new EffectParameters(param1, param2, param3, param4);

            yield return GetEffect(id, parameters, 0, 0, [], EffectAreaManager.DefaultArea);
        }
    }
}
