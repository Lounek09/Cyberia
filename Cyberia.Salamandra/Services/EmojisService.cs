using Cyberia.Api;

using DSharpPlus;
using DSharpPlus.Entities;

using System.Collections.Frozen;

namespace Cyberia.Salamandra.Services;

/// <summary>
/// Represents a service for dealing with discord emojis.
/// </summary>
public sealed class EmojisService
{
    private static FrozenDictionary<string, DiscordEmoji> s_cachedEmojis = FrozenDictionary<string, DiscordEmoji>.Empty;

    private readonly DiscordClient _discordClient;
    private readonly HttpClient _httpClient;

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

    public static string GetEmojiStringByName(string name)
    {
        s_cachedEmojis.TryGetValue(name, out var emoji);
        return emoji?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// Loads the emojis in memory.
    /// </summary>
    public async Task LoadEmojisAsync()
    {
        var emojis = await _discordClient.GetApplicationEmojisAsync();

        s_cachedEmojis = emojis.ToFrozenDictionary(x => x.Name, x => x);
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
            var emojiName = $"effect_{effectData.GetIconId()}";
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
        await CreateEmojiAsync("health", $"{baseRoute}/others/health.png", emojis, checkedEmojiRoutes);
        await CreateEmojiAsync("hourglass", $"{baseRoute}/others/hourglass.png", emojis, checkedEmojiRoutes);
        await CreateEmojiAsync("house", $"{baseRoute}/others/house.png", emojis, checkedEmojiRoutes);
        await CreateEmojiAsync("kamas", $"{baseRoute}/others/kamas.png", emojis, checkedEmojiRoutes);
        await CreateEmojiAsync("mp_resistance", $"{baseRoute}/others/mp_resistance.png", emojis, checkedEmojiRoutes);
        await CreateEmojiAsync("quest", $"{baseRoute}/others/quest.png", emojis, checkedEmojiRoutes);
        await CreateEmojiAsync("repeatable_quest", $"{baseRoute}/others/repeatable_quest.png", emojis, checkedEmojiRoutes);
        await CreateEmojiAsync("true", $"{baseRoute}/others/true.png", emojis, checkedEmojiRoutes);
        await CreateEmojiAsync("unknown", $"{baseRoute}/others/unknown.png", emojis, checkedEmojiRoutes);
        await CreateEmojiAsync("xp", $"{baseRoute}/others/xp.png", emojis, checkedEmojiRoutes);

        s_cachedEmojis = emojis.ToFrozenDictionary();
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
