using Cyberia.Api.Factories.Effects;
using Cyberia.Api.Managers;

using System.Globalization;
using System.Text.Json;

namespace Cyberia.Api.Factories
{
    public sealed record EffectParameters(int Param1, int Param2, int Param3, string Param4);

    public static class EffectFactory
    {
        private static readonly Dictionary<int, Func<int, EffectParameters, int, int, string, Area, IEffect>> _factory = new()
        {
            { -1, PaddockItemEffectivenessEffect.Create },
            { 10, LearnEmoteEffect.Create },
            { 34, QuestEffect.Create },
            { 35, QuestEffect.Create },
            { 165, IncreaseWeaponDamageEffect.Create },
            { 181, SummonMonsterInFightEffect.Create },
            { 185, SummonStaticMonsterInFightEffect.Create },
            { 192, DeleteItemEffect.Create },
            { 193, GiveItemEffect.Create },
            { 197, TransformIntoMonsterEffect.Create },
            { 208, LaunchSpellLevelAnimationEffect.Create },
            { 228, LaunchSpellLevelAnimationEffect.Create },
            { 221, GiveItemEffect.Create },
            { 229, GiveRideEffect.Create },
            { 233, RemoveItemAroundEffect.Create },
            { 239, TransformIntoMonsterEffect.Create },
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
            { 400, TrapSpellEffect.Create },
            { 401, GlyphSpellEffect.Create },
            { 402, GlyphSpellEffect.Create },
            { 405, KIllAndReplaceByMonsterEffect.Create },
            { 601, TeleportToMapEffect.Create },
            { 604, LearnSpellLevelEffect.Create },
            { 614, GiveJobXpEffect.Create },
            { 615, ForgetJobEffect.Create },
            { 616, ForgetSpellLevelEffect.Create },
            { 621, SummonMonsterEffect.Create },
            { 623, SummonMonsterFromSoulStoneEffect.Create },
            { 624, ForgetSpellLevelEffect.Create },
            { 628, SummonMonsterFromSoulGemEffect.Create },
            { 649, AlignmentEffect.Create },
            { 699, LinkJobEffect.Create },
            { 715, MonsterSuperRaceEffect.Create },
            { 716, MonsterRaceEffect.Create },
            { 717, MonsterEffect.Create },
            { 724, ShowTitleEffect.Create },
            { 787, LaunchSpellEffect.Create },
            { 805, ReceivedOnDateTimeEffect.Create },
            { 806, PetCorpulenceEffect.Create },
            { 807, LastMealPetEffect.Create },
            { 808, LastMealDateTimeEffect.Create },
            { 814, KeyEffect.Create },
            { 830, GuildTeleportationEffect.Create },
            { 905, LaunchFightWithMonsterEffect.Create },
            { 939, EnhancePetEffect.Create },
            { 945, GiveRideAbilityEffect.Create },
            { 950, AddStateEffect.Create },
            { 951, RemoveStateEffect.Create },
            { 960, AlignmentEffect.Create },
            { 969, ItemLookEffect.Create },
            { 970, LivingItemEffect.Create },
            { 971, LivingItemCorpulenceEffect.Create },
            { 973, CompatibleWithItemTypeEffect.Create },
            { 983, ExchangeableFromDateTimeEffect.Create },
            { 999, TeleportToMap2Effect.Create },
            { 2101, GiveTTGCardFromPackEffect.Create },
            { 2102, AddTTGCardToBinderEffect.Create },
            { 2128, AddStateEffect.Create },
            { 2129, RemoveStateEffect.Create },
            { 2137, AddStateEffect.Create },
            { 2138, ModifySpellEffect.Create },
            { 2143, TeleportMonsterEffect.Create }
        };

        public static IEffect GetEffect(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area)
        {
            if (_factory.TryGetValue(effectId, out Func<int, EffectParameters, int, int, string, Area, IEffect>? builder))
                return builder(effectId, parameters, duration, probability, criteria, area);

            return BasicEffect.Create(effectId, parameters, duration, probability, criteria, area);
        }

        public static IEnumerable<IEffect> GetEffectsParseFromSpell(JsonElement[] effects, List<Area> areas)
        {
            for (int i = 0; i < effects.Length; i++)
            {
                JsonElement effect = effects[i];

                int id = effect[0].GetInt32();
                int param1 = effect[1].ValueKind is JsonValueKind.Null ? 0 : effect[1].GetInt32();
                int param2 = effect[2].ValueKind is JsonValueKind.Null ? 0 : effect[2].GetInt32();
                int param3 = effect[3].ValueKind is JsonValueKind.Null ? 0 : effect[3].GetInt32();
                string param4 = effect.GetArrayLength() > 7 && effect[7].ValueKind is not JsonValueKind.Null ? effect[7].GetString() ?? "" : "";
                EffectParameters parameters = new(param1, param2, param3, param4);
                int duration = effect[4].ValueKind == JsonValueKind.Null ? 0 : effect[4].GetInt32();
                int probability = effect[5].ValueKind == JsonValueKind.Null ? 0 : effect[5].GetInt32();
                string criteria = effect[6].GetString() ?? "";

                yield return GetEffect(id, parameters, duration, probability, criteria, areas[i]);
            }
        }

        public static IEnumerable<IEffect> GetEffectsParseFromItem(string effects)
        {
            foreach (string effect in effects.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                string[] args = effect.Split("#");

                int id = args[0].StartsWith('-') ? int.Parse(args[0]) : int.Parse(args[0], NumberStyles.HexNumber);
                int param1 = args.Length > 1 && !string.IsNullOrEmpty(args[1]) ? args[1].StartsWith('-') ? int.Parse(args[1]) : int.Parse(args[1], NumberStyles.HexNumber) : 0;
                int param2 = args.Length > 2 && !string.IsNullOrEmpty(args[2]) ? args[1].StartsWith('-') ? int.Parse(args[2]) : int.Parse(args[2], NumberStyles.HexNumber) : 0;
                int param3 = args.Length > 3 && !string.IsNullOrEmpty(args[3]) ? args[1].StartsWith('-') ? int.Parse(args[3]) : int.Parse(args[3], NumberStyles.HexNumber) : 0;
                string param4 = args.Length > 4 ? args[4] : "";
                EffectParameters parameters = new(param1, param2, param3, param4);

                yield return GetEffect(id, parameters, 0, 0, "", EffectAreaManager.BaseArea);
            }
        }
    }
}
