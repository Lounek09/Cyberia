using Cyberia.Api.Data.Emotes;
using Cyberia.Api.Data.Jobs;
using Cyberia.Api.Data.Quests;
using Cyberia.Api.Data.Runes;
using Cyberia.Api.Data.States;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects;
using Cyberia.Api.Factories.Effects.Elements;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Salamandra.Services;

using System.Globalization;

namespace Cyberia.Salamandra;

public static class Emojis
{
    public static string Bool(bool value, CultureInfo? culture)
    {
        return value
            ? EmojisService.GetEmojiStringByName("true", Translation.Get<BotTranslations>("True", culture))
            : EmojisService.GetEmojiStringByName("false", Translation.Get<BotTranslations>("False", culture));
    }

    // EffectAreas
    public static string EffectArea(EffectArea effectArea, CultureInfo? culture)
    {
        var emoji = EmojisService.GetEmojiStringByName($"effectarea_{effectArea.Id}", effectArea.GetName(culture));
        if (string.IsNullOrEmpty(emoji))
        {
            return Unknown(culture);
        }

        return emoji;
    }

    // Effects
    public static string Effect(IEffect effect, CultureInfo? culture)
    {
        var emoji = effect switch
        {
            CharacterLearnEmoteEffect characterLearnEmoteEffect => Emote(characterLearnEmoteEffect.GetEmoteData(), culture),
            RideDetailsEffect => RideDetail(culture),
            ICharacteristicEffect characteristicEffect => EmojisService.GetEmojiStringByName($"effect_{characteristicEffect.CharacteristicId}", ReadOnlySpan<char>.Empty),
            IJobEffect jobEffect => Job(jobEffect.GetJobData(), culture),
            IStateEffect stateEffect => State(stateEffect.GetStateData(), culture),
            ISpellEffect or ISpellLevelEffect => Spell(culture),
            _ => null
        };

        if (emoji is null)
        {
            var effectData = effect.GetEffectData();
            if (effectData is null)
            {
                return Empty(culture);
            }

            emoji = EmojisService.GetEmojiStringByName($"effect_{effectData.GfxId}", ReadOnlySpan<char>.Empty);
        }

        if (string.IsNullOrEmpty(emoji))
        {
            return Empty(culture);
        }

        return emoji;
    }

    // Emotes
    public static string Emote(EmoteData? emoteData, CultureInfo? culture)
    {
        if (emoteData is null)
        {
            return Empty(culture);
        }

        var emoji = EmojisService.GetEmojiStringByName($"emote_{emoteData.Id}", emoteData.Name.ToString(culture));
        if (string.IsNullOrEmpty(emoji))
        {
            return Empty(culture);
        }

        return emoji;
    }

    // Jobs
    public static string Job(JobData? jobData, CultureInfo? culture)
    {
        if (jobData is null)
        {
            return Empty(culture);
        }

        var emoji = EmojisService.GetEmojiStringByName($"job_{jobData.Id}", jobData.Name.ToString(culture));
        if (string.IsNullOrEmpty(emoji))
        {
            return Empty(culture);
        }

        return emoji;
    }

    // Quests
    public static string Quest(QuestData questData, CultureInfo? culture)
    {
        if (questData.Repeatable && questData.Account)
        {
            return EmojisService.GetEmojiStringByName("repeatable_quest", Translation.Get<BotTranslations>("RepeatableQuest", culture)) +
                EmojisService.GetEmojiStringByName("account_quest", Translation.Get<BotTranslations>("AccountQuest", culture));
        }

        if (questData.Account)
        {
            return EmojisService.GetEmojiStringByName("account_quest", Translation.Get<BotTranslations>("AccountQuest", culture));
        }

        if (questData.Repeatable)
        {
            return EmojisService.GetEmojiStringByName("repeatable_quest", Translation.Get<BotTranslations>("RepeatableQuest", culture));
        }

        return EmojisService.GetEmojiStringByName("quest", Translation.Get<BotTranslations>("Quest", culture));
    }

    // Runes
    public static string BaRune(RuneData? runeData, CultureInfo? culture)
    {
        var itemData = runeData?.GetBaRuneItemData();
        if (itemData is null)
        {
            return Empty(culture);
        }

        var emoji = EmojisService.GetEmojiStringByName($"item_{itemData.ItemTypeId}_{itemData.GfxId}", itemData.Name.ToString(culture));
        if (string.IsNullOrEmpty(emoji))
        {
            return Empty(culture);
        }

        return emoji;
    }

    public static string PaRune(RuneData? runeData, CultureInfo? culture)
    {
        var itemData = runeData?.GetPaRuneItemData();
        if (itemData is null)
        {
            return Empty(culture);
        }

        var emoji = EmojisService.GetEmojiStringByName($"item_{itemData.ItemTypeId}_{itemData.GfxId}", itemData.Name.ToString(culture));
        if (string.IsNullOrEmpty(emoji))
        {
            return Empty(culture);
        }

        return emoji;
    }

    public static string RaRune(RuneData? runeData, CultureInfo? culture)
    {
        var itemData = runeData?.GetRaRuneItemData();
        if (itemData is null)
        {
            return Empty(culture);
        }

        var emoji = EmojisService.GetEmojiStringByName($"item_{itemData.ItemTypeId}_{itemData.GfxId}", itemData.Name.ToString(culture));
        if (string.IsNullOrEmpty(emoji))
        {
            return Empty(culture);
        }

        return emoji;
    }

    // States
    public static string State(StateData? stateData, CultureInfo? culture)
    {
        if (stateData is null)
        {
            return Empty(culture);
        }

        var emoji = EmojisService.GetEmojiStringByName($"state_{stateData.Id}", stateData.Name.ToString(culture));
        if (string.IsNullOrEmpty(emoji))
        {
            return Empty(culture);
        }

        return emoji;
    }

    // Others
    public static string ActionPoint(CultureInfo? culture)
    {
        return EmojisService.GetEmojiStringByName("effect_1", Translation.Get<BotTranslations>("ShortActionPoint", culture));
    }

    public static string ActionPointResistance(CultureInfo? culture)
    {
        return EmojisService.GetEmojiStringByName("ap_resistance", Translation.Get<BotTranslations>("ActionPointResistance", culture));
    }

    public static string AirResistance(CultureInfo? culture)
    {
        return EmojisService.GetEmojiStringByName("effect_36", Translation.Get<BotTranslations>("AirResistance", culture));
    }

    public static string Dispellable(CultureInfo? culture)
    {
        return EmojisService.GetEmojiStringByName("lock", Translation.Get<BotTranslations>("NotDispellable", culture));
    }

    public static string Dungeon(CultureInfo? culture)
    {
        return EmojisService.GetEmojiStringByName("dungeon", Translation.Get<BotTranslations>("Dungeon", culture));
    }

    public static string EarthResistance(CultureInfo? culture)
    {
        return EmojisService.GetEmojiStringByName("effect_33", Translation.Get<BotTranslations>("EarthResistance", culture));
    }

    public static string Empty(CultureInfo? culture)
    {
        return EmojisService.GetEmojiStringByName("empty", Translation.Get<BotTranslations>("Empty", culture));
    }

    public static string FireResistance(CultureInfo? culture)
    {
        return EmojisService.GetEmojiStringByName("effect_34", Translation.Get<BotTranslations>("FireResistance", culture));
    }

    public static string HealthPoint(CultureInfo? culture)
    {
        return EmojisService.GetEmojiStringByName("health", Translation.Get<BotTranslations>("Vitality", culture));
    }

    public static string House(CultureInfo? culture)
    {
        return EmojisService.GetEmojiStringByName("house", Translation.Get<BotTranslations>("House", culture));
    }

    public static string Kamas(CultureInfo? culture)
    {
        return EmojisService.GetEmojiStringByName("kamas", Translation.Get<BotTranslations>("Kamas", culture));
    }

    public static string MovementPoint(CultureInfo? culture)
    {
        return EmojisService.GetEmojiStringByName("effect_23", Translation.Get<BotTranslations>("ShortMovementPoint", culture));
    }

    public static string MovementPointResistance(CultureInfo? culture)
    {
        return EmojisService.GetEmojiStringByName("mp_resistance", Translation.Get<BotTranslations>("MovementPointResistance", culture));
    }

    public static string NeutralResistance(CultureInfo? culture)
    {
        return EmojisService.GetEmojiStringByName("effect_37", Translation.Get<BotTranslations>("NeutralResistance", culture));
    }

    public static string RideDetail(CultureInfo? culture)
    {
        return EmojisService.GetEmojiStringByName("hand", Translation.Get<BotTranslations>("RideDetail", culture));
    }

    public static string Spell(CultureInfo? culture)
    {
        return EmojisService.GetEmojiStringByName("wand", Translation.Get<BotTranslations>("Spell", culture));
    }

    public static string Unknown(CultureInfo? culture)
    {
        return EmojisService.GetEmojiStringByName("unknown", Translation.Get<BotTranslations>("Unknown", culture));
    }

    public static string WaterResistance(CultureInfo? culture)
    {
        return EmojisService.GetEmojiStringByName("effect_35", Translation.Get<BotTranslations>("WaterResistance", culture));
    }

    public static string Xp(CultureInfo? culture)
    {
        return EmojisService.GetEmojiStringByName("xp", Translation.Get<BotTranslations>("Experience", culture));
    }
}
