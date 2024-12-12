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
            RideDetailsEffect => Hand,
            ICharacteristicEffect characteristicEffect => EmojisService.GetEmojiStringByName($"effect_{characteristicEffect.CharacteristicId}"),
            IJobEffect jobEffect => Job(jobEffect.GetJobData()),
            IStateEffect stateEffect => State(stateEffect.GetStateData()),
            ISpellEffect or ISpellLevelEffect => Wand,
            _ => null
        };

        if (emoji is null)
        {
            var effectData = effect.GetEffectData();
            if (effectData is null)
            {
                return Empty;
            }

            emoji = EmojisService.GetEmojiStringByName($"effect_{effectData.GfxId}");
        }

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
    public static string BaRune(RuneData? runeData)
    {
        var itemData = runeData?.GetBaRuneItemData();
        if (itemData is null)
        {
            return Empty;
        }

        var emoji = EmojisService.GetEmojiStringByName($"item_{itemData.ItemTypeId}_{itemData.GfxId}");
        if (string.IsNullOrEmpty(emoji))
        {
            return Empty;
        }

        return emoji;
    }

    public static string PaRune(RuneData? runeData)
    {
        var itemData = runeData?.GetPaRuneItemData();
        if (itemData is null)
        {
            return Empty;
        }

        var emoji = EmojisService.GetEmojiStringByName($"item_{itemData.ItemTypeId}_{itemData.GfxId}");
        if (string.IsNullOrEmpty(emoji))
        {
            return Empty;
        }

        return emoji;
    }

    public static string RaRune(RuneData? runeData)
    {
        var itemData = runeData?.GetRaRuneItemData();
        if (itemData is null)
        {
            return Empty;
        }

        var emoji = EmojisService.GetEmojiStringByName($"item_{itemData.ItemTypeId}_{itemData.GfxId}");
        if (string.IsNullOrEmpty(emoji))
        {
            return Empty;
        }

        return emoji;
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
    public static string ActionPoint => EmojisService.GetEmojiStringByName("effect_1");
    public static string AirResistance => EmojisService.GetEmojiStringByName("effect_36");
    public static string ApResistance => EmojisService.GetEmojiStringByName("ap_resistance");
    public static string Dungeon => EmojisService.GetEmojiStringByName("dungeon");
    public static string EarthResistance => EmojisService.GetEmojiStringByName("effect_33");
    public static string Empty => EmojisService.GetEmojiStringByName("empty");
    public static string FireResistance => EmojisService.GetEmojiStringByName("effect_34");
    public static string Hand => EmojisService.GetEmojiStringByName("hand");
    public static string HealthPoint => EmojisService.GetEmojiStringByName("health");
    public static string House => EmojisService.GetEmojiStringByName("house");
    public static string Kamas => EmojisService.GetEmojiStringByName("kamas");
    public static string MovementPoint => EmojisService.GetEmojiStringByName("effect_23");
    public static string MpResistance => EmojisService.GetEmojiStringByName("mp_resistance");
    public static string NeutralResistance => EmojisService.GetEmojiStringByName("effect_37");
    public static string Unknown => EmojisService.GetEmojiStringByName("unknown");
    public static string Wand => EmojisService.GetEmojiStringByName("wand");
    public static string WaterResistance => EmojisService.GetEmojiStringByName("effect_35");
    public static string Xp => EmojisService.GetEmojiStringByName("xp");
}
