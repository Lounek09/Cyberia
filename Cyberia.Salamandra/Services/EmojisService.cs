using Cyberia.Api;
using Cyberia.Api.Data;

using DSharpPlus;
using DSharpPlus.Entities;

using System.Buffers;

namespace Cyberia.Salamandra.Services;

/// <summary>
/// Represents a service for dealing with discord emojis.
/// </summary>
public interface IEmojisService
{
    /// <summary>
    /// Loads the emojis in memory.
    /// </summary>
    Task LoadEmojisAsync();

    /// <summary>
    /// Deletes the emoji with the specified name.
    /// </summary>
    /// <param name="name">The name of the emoji.</param>
    /// <returns><see langword="true"/> if the emoji was deleted, <see langword="false"/> otherwise.</returns>
    Task<bool> DeleteEmojiAsync(string name);

    /// <summary>
    /// Creates the missing emojis based on the Dofus API data and the images in the CDN.
    /// </summary>
    Task CreateEmojisAsync();
}

public sealed class EmojisService : IEmojisService
{
    private static Dictionary<string, DiscordEmoji> s_cachedEmojis = [];
    private static readonly SearchValues<char> s_authorizedChars = SearchValues.Create("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_");

    private readonly DiscordClient _discordClient;
    private readonly DofusDatacenter _dofusDatacenter;
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of <see cref="EmojisService"/> class.
    /// </summary>
    /// <param name="discordClient">The discord client.</param>
    /// <param name="dofusApiConfig">The Dofus API configuration.</param>
    /// <param name="dofusDatacenter">The Dofus datacenter.</param>
    public EmojisService(DiscordClient discordClient, DofusApiConfig dofusApiConfig, DofusDatacenter dofusDatacenter)
    {
        _dofusDatacenter = dofusDatacenter;
        _discordClient = discordClient;
        _httpClient = new()
        {
            BaseAddress = new(dofusApiConfig.CdnUrl)
        };
    }

    /// <summary>
    /// Gets the emojis that contain the specified name.
    /// </summary>
    /// <param name="name">The name to search for, if empty all emojis will be returned.</param>
    /// <returns>An enumerable of emojis that contain the specified name.</returns>
    public static IEnumerable<DiscordEmoji> GetEmojisByName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return s_cachedEmojis.Values;
        }

        return s_cachedEmojis.Values.Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Gets the string representation of the emoji with the specified name.
    /// </summary>
    /// <param name="name">The name of the emoji.</param>
    /// <param name="displayedName">The displayed name of the emoji, if empty the default name will be used.</param>
    /// <returns>The string representation of the emoji, or an empty string if the emoji was not found.</returns>
    public static string GetEmojiStringByName(string name, ReadOnlySpan<char> displayedName)
    {
        if (!s_cachedEmojis.TryGetValue(name, out var emoji))
        {
            return string.Empty;
        }

        if (displayedName.IsEmpty)
        {
            return emoji.ToString();
        }

        var normalizedName = displayedName.NormalizeToAscii();

        Span<char> sanitizedNameBuffer = stackalloc char[displayedName.Length];
        var validCharCount = 0;

        foreach (var c in displayedName)
        {
            if (s_authorizedChars.Contains(c))
            {
                sanitizedNameBuffer[validCharCount++] = c;
            }
            else if (c == ' ')
            {
                sanitizedNameBuffer[validCharCount++] = '_';
            }
        }

        ReadOnlySpan<char> sanitizedName = sanitizedNameBuffer[..validCharCount];

        return emoji.IsAnimated
            ? $"<a:{sanitizedName}:{emoji.Id}>"
            : $"<:{sanitizedName}:{emoji.Id}>";
    }

    public async Task LoadEmojisAsync()
    {
        var emojis = await _discordClient.GetApplicationEmojisAsync();

        s_cachedEmojis = emojis.ToDictionary(x => x.Name, x => x);
    }

    public async Task<bool> DeleteEmojiAsync(string name)
    {
        if (!s_cachedEmojis.TryGetValue(name, out var emoji))
        {
            return false;
        }

        await _discordClient.DeleteApplicationEmojiAsync(emoji.Id);
        return s_cachedEmojis.Remove(name);
    }

    public async Task CreateEmojisAsync()
    {
        const string baseRoute = "/images/discord/emojis";

        var emojis = await _discordClient.GetApplicationEmojisAsync();
        var emojisDictionary = emojis.ToDictionary(x => x.Name, x => x);
        HashSet<string> checkedEmojiRoutes = [];

        // EffectAreas
        foreach (var itemTypeData in _dofusDatacenter.ItemsRepository.ItemTypes.Values)
        {
            var emojiName = $"effectarea_{itemTypeData.EffectArea.Id}";
            var emojiRoute = $"{baseRoute}/effectareas/{emojiName}.png";

            await CreateEmojiAsync(emojiName, emojiRoute);
        }

        foreach (var spellData in _dofusDatacenter.SpellsRepository.Spells.Values)
        {
            foreach (var spellLevelData in spellData.GetSpellLevelsData())
            {
                foreach (var effect in spellLevelData.Effects)
                {
                    var emojiName = $"effectarea_{effect.EffectArea.Id}";
                    var emojiRoute = $"{baseRoute}/effectareas/{emojiName}.png";

                    await CreateEmojiAsync(emojiName, emojiRoute);
                }

                foreach (var effect in spellLevelData.CriticalEffects)
                {
                    var emojiName = $"effectarea_{effect.EffectArea.Id}";
                    var emojiRoute = $"{baseRoute}/effectareas/{emojiName}.png";

                    await CreateEmojiAsync(emojiName, emojiRoute);
                }
            }
        }

        // Effects
        foreach (var effectData in _dofusDatacenter.EffectsRepository.Effects.Values)
        {
            var emojiName = $"effect_{effectData.GfxId}";
            var emojiRoute = $"{baseRoute}/effects/{emojiName}.png";

            await CreateEmojiAsync(emojiName, emojiRoute);
        }

        // Emotes
        foreach (var emoteData in _dofusDatacenter.EmotesRepository.Emotes.Values)
        {
            var emojiName = $"emote_{emoteData.Id}";
            var emojiRoute = $"{baseRoute}/emotes/{emojiName}.png";

            await CreateEmojiAsync(emojiName, emojiRoute);
        }

        // Jobs
        foreach (var jobData in _dofusDatacenter.JobsRepository.Jobs.Values)
        {
            var emojiName = $"job_{jobData.Id}";
            var emojiRoute = $"{baseRoute}/jobs/{emojiName}.png";

            await CreateEmojiAsync(emojiName, emojiRoute);
        }

        // Items
        foreach (var runeData in _dofusDatacenter.RunesRepository.Runes.Values)
        {
            var baRuneItemData = runeData.GetBaRuneItemData();
            if (baRuneItemData is not null)
            {
                var emojiName = $"item_{baRuneItemData.ItemTypeId}_{baRuneItemData.GfxId}";
                var emojiRoute = $"{baseRoute}/items/{baRuneItemData.ItemTypeId}/{emojiName}.png";

                await CreateEmojiAsync(emojiName, emojiRoute);
            }

            var paRuneItemData = runeData.GetPaRuneItemData();
            if (paRuneItemData is not null)
            {
                var emojiName = $"item_{paRuneItemData.ItemTypeId}_{paRuneItemData.GfxId}";
                var emojiRoute = $"{baseRoute}/items/{paRuneItemData.ItemTypeId}/{emojiName}.png";

                await CreateEmojiAsync(emojiName, emojiRoute);
            }

            var raRuneItemData = runeData.GetRaRuneItemData();
            if (raRuneItemData is not null)
            {
                var emojiName = $"item_{raRuneItemData.ItemTypeId}_{raRuneItemData.GfxId}";
                var emojiRoute = $"{baseRoute}/items/{raRuneItemData.ItemTypeId}/{emojiName}.png";

                await CreateEmojiAsync(emojiName, emojiRoute);
            }
        }

        // States
        foreach (var stateData in _dofusDatacenter.StatesRepository.States.Values)
        {
            var emojiName = $"state_{stateData.Id}";
            var emojiRoute = $"{baseRoute}/states/{emojiName}.png";

            await CreateEmojiAsync(emojiName, emojiRoute);
        }

        // Others
        await CreateEmojiAsync("account_quest", $"{baseRoute}/others/account_quest.png");
        await CreateEmojiAsync("ap_resistance", $"{baseRoute}/others/ap_resistance.png");
        await CreateEmojiAsync("dungeon", $"{baseRoute}/others/dungeon.png" );
        await CreateEmojiAsync("empty", $"{baseRoute}/others/empty.png");
        await CreateEmojiAsync("false", $"{baseRoute}/others/false.png");
        await CreateEmojiAsync("hand", $"{baseRoute}/others/hand.png");
        await CreateEmojiAsync("health", $"{baseRoute}/others/health.png");
        await CreateEmojiAsync("hourglass", $"{baseRoute}/others/hourglass.png");
        await CreateEmojiAsync("house", $"{baseRoute}/others/house.png");
        await CreateEmojiAsync("kamas", $"{baseRoute}/others/kamas.png");
        await CreateEmojiAsync("lock", $"{baseRoute}/others/lock.png");
        await CreateEmojiAsync("mp_resistance", $"{baseRoute}/others/mp_resistance.png");
        await CreateEmojiAsync("quest", $"{baseRoute}/others/quest.png");
        await CreateEmojiAsync("repeatable_quest", $"{baseRoute}/others/repeatable_quest.png");
        await CreateEmojiAsync("true", $"{baseRoute}/others/true.png");
        await CreateEmojiAsync("unknown", $"{baseRoute}/others/unknown.png");
        await CreateEmojiAsync("wand", $"{baseRoute}/others/wand.png");
        await CreateEmojiAsync("xp", $"{baseRoute}/others/xp.png");

        s_cachedEmojis = emojisDictionary;

        async Task CreateEmojiAsync(string emojiName, string emojiRoute)
        {
            if (emojisDictionary.ContainsKey(emojiName) || checkedEmojiRoutes.Contains(emojiRoute))
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
            emojisDictionary[emojiName] = emoji;
        }
    }

    /// <summary>
    /// Gets the image stream from the specified route.
    /// </summary>
    /// <param name="route">The route of the image.</param>
    /// <returns>The image stream, or <see langword="null"/> if the image does not exist.</returns>
    private async Task<Stream?> GetImageStreamAsync(string route)
    {
        try
        {
            var bytes = await _httpClient.GetByteArrayAsync(route);

            return new MemoryStream(bytes);
        }
        catch
        {
            return null;
        }
    }
}
