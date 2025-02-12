using Cyberia.Api;

using DSharpPlus;
using DSharpPlus.Entities;

using System.Text.RegularExpressions;

namespace Cyberia.Salamandra.Services;

/// <summary>
/// Represents a service for dealing with discord emojis.
/// </summary>
public sealed partial class EmojisService
{
    private static Dictionary<string, DiscordEmoji> s_cachedEmojis = [];

    private readonly DiscordClient _discordClient;
    private readonly HttpClient _httpClient;

    [GeneratedRegex(@"[^\w]")]
    private static partial Regex EmojiNameSanitizationRegex();

    /// <summary>
    /// Initializes a new instance of <see cref="EmojisService"/>.
    /// </summary>
    /// <param name="discordClient">The discord client.</param>
    public EmojisService(DiscordClient discordClient)
    {
        _discordClient = discordClient;
        _httpClient = new()
        {
            BaseAddress = new(DofusApi.Config.CdnUrl)
        };
    }

    /// <summary>
    /// Gets the emojis that contain the specified name.
    /// </summary>
    /// <param name="name">The name to search for.</param>
    /// <returns>An enumerable of emojis that contain the specified name.</returns>
    public static IEnumerable<DiscordEmoji> GetEmojisByName(string name)
    {
        return s_cachedEmojis.Values.Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Gets the string representation of the emoji with the specified name.
    /// </summary>
    /// <param name="name">The name of the emoji.</param>
    /// <param name="displayedName">The displayed name of the emoji.</param>
    /// <returns>The string representation of the emoji, or an empty string if the emoji was not found.</returns>
    public static string GetEmojiStringByName(string name, string? displayedName = null)
    {
        if (!s_cachedEmojis.TryGetValue(name, out var emoji))
        {
            return string.Empty;
        }

        if (displayedName is null)
        {
            return emoji.ToString();
        }

        displayedName = displayedName.NormalizeToAscii();
        displayedName = displayedName.Replace(' ', '_');
        displayedName = EmojiNameSanitizationRegex().Replace(displayedName, string.Empty);

        var displayedNameSpan = displayedName.AsSpan(..Math.Min(displayedName.Length, Constant.MaxEmojiNameSize));

        return emoji.IsAnimated
            ? $"<a:{displayedNameSpan}:{emoji.Id}>"
            : $"<:{displayedNameSpan}:{emoji.Id}>";
    }

    /// <summary>
    /// Loads the emojis in memory.
    /// </summary>
    public async Task LoadEmojisAsync()
    {
        var emojis = await _discordClient.GetApplicationEmojisAsync();

        s_cachedEmojis = emojis.ToDictionary(x => x.Name, x => x);
    }

    /// <summary>
    /// Deletes the emoji with the specified name.
    /// </summary>
    /// <param name="name">The name of the emoji.</param>
    /// <returns><see langword="true"/> if the emoji was deleted, <see langword="false"/> otherwise.</returns>
    public async Task<bool> DeleteEmojiAsync(string name)
    {
        if (!s_cachedEmojis.TryGetValue(name, out var emoji))
        {
            return false;
        }

        try
        {
            await _discordClient.DeleteApplicationEmojiAsync(emoji.Id);
            return s_cachedEmojis.Remove(name);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Updates the emojis based on the datacenter for the discord client.
    /// </summary>
    public async Task UpdateEmojisAsync()
    {
        const string baseRoute = "/images/discord/emojis";

        HashSet<string> checkedEmojiRoutes = [];
        var emojis = (await _discordClient.GetApplicationEmojisAsync()).ToDictionary(x => x.Name, x => x); 

        // EffectAreas
        foreach (var itemTypeData in DofusApi.Datacenter.ItemsRepository.ItemTypes.Values)
        {
            var emojiName = $"effectarea_{itemTypeData.EffectArea.Id}";
            var emojiRoute = $"{baseRoute}/effectareas/{emojiName}.png";

            await CreateEmojiAsync(emojiName, emojiRoute, emojis, checkedEmojiRoutes);
        }

        foreach (var spellData in DofusApi.Datacenter.SpellsRepository.Spells.Values)
        {
            foreach (var spellLevelData in spellData.GetSpellLevelsData())
            {
                foreach (var effect in spellLevelData.Effects)
                {
                    var emojiName = $"effectarea_{effect.EffectArea.Id}";
                    var emojiRoute = $"{baseRoute}/effectareas/{emojiName}.png";

                    await CreateEmojiAsync(emojiName, emojiRoute, emojis, checkedEmojiRoutes);
                }

                foreach (var effect in spellLevelData.CriticalEffects)
                {
                    var emojiName = $"effectarea_{effect.EffectArea.Id}";
                    var emojiRoute = $"{baseRoute}/effectareas/{emojiName}.png";

                    await CreateEmojiAsync(emojiName, emojiRoute, emojis, checkedEmojiRoutes);
                }
            }
        }

        // Effects
        foreach (var effectData in DofusApi.Datacenter.EffectsRepository.Effects.Values)
        {
            var emojiName = $"effect_{effectData.GfxId}";
            var emojiRoute = $"{baseRoute}/effects/{emojiName}.png";

            await CreateEmojiAsync(emojiName, emojiRoute, emojis, checkedEmojiRoutes);
        }

        // Emotes
        foreach (var emoteData in DofusApi.Datacenter.EmotesRepository.Emotes.Values)
        {
            var emojiName = $"emote_{emoteData.Id}";
            var emojiRoute = $"{baseRoute}/emotes/{emojiName}.png";

            await CreateEmojiAsync(emojiName, emojiRoute, emojis, checkedEmojiRoutes);
        }

        // Jobs
        foreach (var jobData in DofusApi.Datacenter.JobsRepository.Jobs.Values)
        {
            var emojiName = $"job_{jobData.Id}";
            var emojiRoute = $"{baseRoute}/jobs/{emojiName}.png";

            await CreateEmojiAsync(emojiName, emojiRoute, emojis, checkedEmojiRoutes);
        }

        // Items
        foreach (var runeData in DofusApi.Datacenter.RunesRepository.Runes.Values)
        {
            var baRuneItemData = runeData.GetBaRuneItemData();
            if (baRuneItemData is not null)
            {
                var emojiName = $"item_{baRuneItemData.ItemTypeId}_{baRuneItemData.GfxId}";
                var emojiRoute = $"{baseRoute}/items/{baRuneItemData.ItemTypeId}/{emojiName}.png";

                await CreateEmojiAsync(emojiName, emojiRoute, emojis, checkedEmojiRoutes);
            }

            var paRuneItemData = runeData.GetPaRuneItemData();
            if (paRuneItemData is not null)
            {
                var emojiName = $"item_{paRuneItemData.ItemTypeId}_{paRuneItemData.GfxId}";
                var emojiRoute = $"{baseRoute}/items/{paRuneItemData.ItemTypeId}/{emojiName}.png";

                await CreateEmojiAsync(emojiName, emojiRoute, emojis, checkedEmojiRoutes);
            }

            var raRuneItemData = runeData.GetRaRuneItemData();
            if (raRuneItemData is not null)
            {
                var emojiName = $"item_{raRuneItemData.ItemTypeId}_{raRuneItemData.GfxId}";
                var emojiRoute = $"{baseRoute}/items/{raRuneItemData.ItemTypeId}/{emojiName}.png";

                await CreateEmojiAsync(emojiName, emojiRoute, emojis, checkedEmojiRoutes);
            }
        }

        // States
        foreach (var stateData in DofusApi.Datacenter.StatesRepository.States.Values)
        {
            var emojiName = $"state_{stateData.Id}";
            var emojiRoute = $"{baseRoute}/states/{emojiName}.png";

            await CreateEmojiAsync(emojiName, emojiRoute, emojis, checkedEmojiRoutes);
        }

        // Others
        await CreateEmojiAsync("account_quest", $"{baseRoute}/others/account_quest.png", emojis, checkedEmojiRoutes);
        await CreateEmojiAsync("ap_resistance", $"{baseRoute}/others/ap_resistance.png", emojis, checkedEmojiRoutes);
        await CreateEmojiAsync("dungeon", $"{baseRoute}/others/dungeon.png", emojis, checkedEmojiRoutes);
        await CreateEmojiAsync("empty", $"{baseRoute}/others/empty.png", emojis, checkedEmojiRoutes);
        await CreateEmojiAsync("false", $"{baseRoute}/others/false.png", emojis, checkedEmojiRoutes);
        await CreateEmojiAsync("hand", $"{baseRoute}/others/hand.png", emojis, checkedEmojiRoutes);
        await CreateEmojiAsync("health", $"{baseRoute}/others/health.png", emojis, checkedEmojiRoutes);
        await CreateEmojiAsync("hourglass", $"{baseRoute}/others/hourglass.png", emojis, checkedEmojiRoutes);
        await CreateEmojiAsync("house", $"{baseRoute}/others/house.png", emojis, checkedEmojiRoutes);
        await CreateEmojiAsync("kamas", $"{baseRoute}/others/kamas.png", emojis, checkedEmojiRoutes);
        await CreateEmojiAsync("lock", $"{baseRoute}/others/lock.png", emojis, checkedEmojiRoutes);
        await CreateEmojiAsync("mp_resistance", $"{baseRoute}/others/mp_resistance.png", emojis, checkedEmojiRoutes);
        await CreateEmojiAsync("quest", $"{baseRoute}/others/quest.png", emojis, checkedEmojiRoutes);
        await CreateEmojiAsync("repeatable_quest", $"{baseRoute}/others/repeatable_quest.png", emojis, checkedEmojiRoutes);
        await CreateEmojiAsync("true", $"{baseRoute}/others/true.png", emojis, checkedEmojiRoutes);
        await CreateEmojiAsync("unknown", $"{baseRoute}/others/unknown.png", emojis, checkedEmojiRoutes);
        await CreateEmojiAsync("wand", $"{baseRoute}/others/wand.png", emojis, checkedEmojiRoutes);
        await CreateEmojiAsync("xp", $"{baseRoute}/others/xp.png", emojis, checkedEmojiRoutes);

        s_cachedEmojis = emojis.ToDictionary();
    }

    /// <summary>
    /// Creates an emoji with the specified name with an image from the specified route on the cdn.
    /// </summary>
    /// <param name="emojiName">The name of the emoji.</param>
    /// <param name="emojiRoute">The route of the image on the cdn.</param>
    /// <param name="emojis">The current created emojis.</param>
    /// <param name="checkedEmojiRoutes">The checked emoji routes.</param>
    private async Task CreateEmojiAsync(string emojiName, string emojiRoute, Dictionary<string, DiscordEmoji> emojis, HashSet<string> checkedEmojiRoutes)
    {
        if (emojis.ContainsKey(emojiName) || checkedEmojiRoutes.Contains(emojiRoute))
        {
            return;
        }

        checkedEmojiRoutes.Add(emojiRoute);

        using var emojiStream = await GetImageStreamAsync(emojiRoute);
        if (emojiStream is null)
        {
            return;
        }

        var emoji = await _discordClient.CreateApplicationEmojiAsync(emojiName, emojiStream);
        emojis.Add(emojiName, emoji);
    }

    /// <summary>
    /// Gets a stream of the image from the specified route.
    /// </summary>
    /// <param name="route">The route of the image on the cdn.</param>
    /// <returns>The stream of the image, or <see langword="null"/> if the image could not be retrieved.</returns>
    private async Task<Stream?> GetImageStreamAsync(string route)
    {
        try
        {
            using var responseStream = await _httpClient.GetStreamAsync(route);

            MemoryStream memoryStream = new();
            await responseStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            return memoryStream;
        }
        catch
        {
            return null;
        }
    }
}
