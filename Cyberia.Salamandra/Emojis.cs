using Cyberia.Api.Data.Emotes;
using Cyberia.Api.Data.Jobs;
using Cyberia.Api.Data.Quests;
using Cyberia.Api.Data.States;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects;
using Cyberia.Api.Factories.Effects.Elements;
using Cyberia.Salamandra.Services;

using System.Collections.Frozen;

namespace Cyberia.Salamandra;

public static class Emojis
{
    // Boolean
    private static string True => EmojisService.GetEmojiStringByName("true");
    private static string False => EmojisService.GetEmojiStringByName("false");

    public static string Bool(bool value)
    {
        return value ? True : False;
    }

    // EffectAreas
    public static string EffectArea(EffectArea effectArea)
    {
        var emoji = EmojisService.GetEmojiStringByName($"effectarea_{effectArea.Id}");
        if (string.IsNullOrEmpty(emoji))
        {
            return Unknown;
        }

        return emoji;
    }

    // Effects
    public static string Effect(IEffect effect)
    {
        var emoji = effect switch
        {
            CharacterLearnEmoteEffect characterLearnEmoteEffect => Emote(characterLearnEmoteEffect.GetEmoteData()),
            FightSetStateEffect fightSetStateEffect => State(fightSetStateEffect.GetStateData()),
            FightUnsetStateEffect fightUnsetStateEffect => State(fightUnsetStateEffect.GetStateData()),
            CharacterGainJobXpEffect characterGainJobXpEffect => Job(characterGainJobXpEffect.GetJobData()),
            CharacterLearnJobEffect characterLearnJobEffect => Job(characterLearnJobEffect.GetJobData()),
            CharacterUnlearnJobEffect characterUnlearnJobEffect => Job(characterUnlearnJobEffect.GetJobData()),
            _ => null
        };

        if (!string.IsNullOrEmpty(emoji))
        {
            return emoji;
        }

        var effectData = effect.GetEffectData();
        if (effectData is null)
        {
            return Empty;
        }

        emoji = EmojisService.GetEmojiStringByName($"effect_{effectData.GetIconId()}");
        if (string.IsNullOrEmpty(emoji))
        {
            return Empty;
        }

        return emoji;
    }

    // Emotes
    public static string Emote(EmoteData? emoteData)
    {
        if (emoteData is null)
        {
            return Empty;
        }

        var emoji = EmojisService.GetEmojiStringByName($"emote_{emoteData.Id}");
        if (string.IsNullOrEmpty(emoji))
        {
            return Empty;
        }

        return emoji;
    }

    // Jobs
    public static string Job(JobData? jobData)
    {
        if (jobData is null)
        {
            return Empty;
        }

        var emoji = EmojisService.GetEmojiStringByName($"job_{jobData.Id}");
        if (string.IsNullOrEmpty(emoji))
        {
            return Empty;
        }

        return emoji;
    }

    // Quests
    private static string ClassicQuest => EmojisService.GetEmojiStringByName("quest");
    private static string RepeatableQuest => EmojisService.GetEmojiStringByName("repeatable_quest");
    private static string AccountQuest => EmojisService.GetEmojiStringByName("account_quest");

    public static string Quest(QuestData questData)
    {
        if (questData.Repeatable && questData.Account)
        {
            return RepeatableQuest + AccountQuest;
        }

        if (questData.Account)
        {
            return AccountQuest;
        }

        if (questData.Repeatable)
        {
            return RepeatableQuest;
        }

        return ClassicQuest;
    }

    // Runes
    private static readonly FrozenDictionary<int, string> s_baRunes = new Dictionary<int, string>()
    {
        { 1, "<:rune_fo:1238075386387234847>" },
        { 2, "<:rune_sa:1238075626095771699>" },
        { 3, "<:rune_ine:1238075428510634036>" },
        { 4, "<:rune_vi:1238075719238549504>" },
        { 5, "<:rune_age:1238075332091838527>" },
        { 6, "<:rune_cha:1238075336252457063>" },
        { 7, "<:rune_ga_pa:1238075387800453130>" },
        { 8, "<:rune_ga_pme:1238075389251944488>" },
        { 9, "<:rune_cri:1238075330607054949>" },
        { 10, "<:rune_so:1238075627714641990>" },
        { 11, "<:rune_do:1238075357345878017>" },
        { 12, "<:rune_do_per:1238075359799414905>" },
        { 13, "<:rune_do_ren:1238075361426669629>" },
        { 14, "<:rune_po:1238075528053919785>" },
        { 15, "<:rune_summo:1238075622048403537>" },
        { 16, "<:rune_pod:1238075560521891920>" },
        { 17, "<:rune_pi:1238075531585519656>" },
        { 18, "<:rune_pi_per:1238075526443307110>" },
        { 19, "<:rune_ini:1238075429768925204>" },
        { 20, "<:rune_prospe:1238075553911803924>" },
        { 21, "<:rune_fire_re:1238075390468034590>" },
        { 22, "<:rune_air_re:1238075333127831663>" },
        { 23, "<:rune_water_re:1238075720429867008>" },
        { 24, "<:rune_earth_re:1238075355374551110>" },
        { 25, "<:rune_neutral_re:1238075431106908160>" },
        { 26, "<:rune_fire_re_per:1238075391676121260>" },
        { 27, "<:rune_air_re_per:1238075334885244928>" },
        { 28, "<:rune_earth_re_per:1238075356502822924>" },
        { 29, "<:rune_neutral_re_per:1238075432062947371>" },
        { 30, "<:rune_water_re_per:1238075717959422002>" },
        { 31, "<:rune_hunting:1238075433602256976>" }
    }.ToFrozenDictionary();

    public static string BaRune(int id)
    {
        return s_baRunes.TryGetValue(id, out var emoji) ? emoji : Empty;
    }

    private static readonly FrozenDictionary<int, string> s_paRunes = new Dictionary<int, string>()
    {
        { 1, "<:rune_pa_fo:1238075466649309235>" },
        { 2, "<:rune_pa_sa:1238075529060417567>" },
        { 3, "<:rune_pa_ine:1238075461028806717>" },
        { 4, "<:rune_pa_vi:1238075530453192756>" },
        { 5, "<:rune_pa_age:1238075462295486464>" },
        { 6, "<:rune_pa_cha:1238075463528747098>" },
        { 12, "<:rune_pa_do_per:1238075465298608139>" },
        { 16, "<:rune_pa_pod:1238075494843289640>" },
        { 17, "<:rune_pa_pi:1238075499553488976>" },
        { 18, "<:rune_pa_pi_per:1238075500648464414>" },
        { 19, "<:rune_pa_ini:1238075497544548404>" },
        { 20, "<:rune_pa_prospe:1238075496349171732>" }
    }.ToFrozenDictionary();

    public static string PaRune(int id)
    {
        return s_paRunes.TryGetValue(id, out var emoji) ? emoji : Empty;
    }

    private static readonly FrozenDictionary<int, string> s_raRunes = new Dictionary<int, string>()
    {
        { 1, "<:rune_ra_fo:1238075593220685866>" },
        { 2, "<:rune_ra_sa:1238075623088459786>" },
        { 3, "<:rune_ra_ine:1238075594537832549>" },
        { 4, "<:rune_ra_vi:1238075624535621702>" },
        { 5, "<:rune_ra_age:1238075556743086121>" },
        { 6, "<:rune_ra_cha:1238075558051577967>" },
        { 12, "<:rune_ra_do_per:1238075559339229194>" },
        { 16, "<:rune_ra_pod:1238075591689900133>" },
        { 18, "<:rune_ra_pi_per:1238075597658525826>" },
        { 19, "<:rune_ra_ini:1238075596060364841>" }
    }.ToFrozenDictionary();

    public static string RaRune(int id)
    {
        return s_raRunes.TryGetValue(id, out var emoji) ? emoji : Empty;
    }

    // States
    public static string State(StateData? stateData)
    {
        if (stateData is null)
        {
            return Empty;
        }

        var emoji = EmojisService.GetEmojiStringByName($"state_{stateData.Id}");
        if (string.IsNullOrEmpty(emoji))
        {
            return Empty;
        }

        return emoji;
    }

    // Others
    public static string HealthPoint => EmojisService.GetEmojiStringByName("health");
    public static string ActionPoint => EmojisService.GetEmojiStringByName("effect_1");
    public static string MovementPoint => EmojisService.GetEmojiStringByName("effect_23");
    public static string ApResistance => EmojisService.GetEmojiStringByName("ap_resistance");
    public static string MpResistance => EmojisService.GetEmojiStringByName("mp_resistance");
    public static string NeutralResistance => EmojisService.GetEmojiStringByName("effect_37");
    public static string EarthResistance => EmojisService.GetEmojiStringByName("effect_33");
    public static string FireResistance => EmojisService.GetEmojiStringByName("effect_34");
    public static string WaterResistance => EmojisService.GetEmojiStringByName("effect_35");
    public static string AirResistance => EmojisService.GetEmojiStringByName("effect_36");
    public static string Empty => EmojisService.GetEmojiStringByName("empty");
    public static string House => EmojisService.GetEmojiStringByName("house");
    public static string Dungeon => EmojisService.GetEmojiStringByName("dungeon");
    public static string Kamas => EmojisService.GetEmojiStringByName("kamas");
    public static string Xp => EmojisService.GetEmojiStringByName("xp");
    public static string Unknown => EmojisService.GetEmojiStringByName("unknown");
}
