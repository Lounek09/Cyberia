using Cyberia.Cytrusaurus;
using Cyberia.Cytrusaurus.EventArgs;
using Cyberia.Cytrusaurus.Extensions;
using Cyberia.Cytrusaurus.Models;
using Cyberia.Salamandra.Extensions.DSharpPlus;

using DSharpPlus;
using DSharpPlus.Entities;

using System.Text;
using System.Text.Json;

namespace Cyberia.Salamandra.Services;

/// <summary>
/// Represents a service to handle Cytrus events and logic.
/// </summary>
public interface ICytrusService
{
    /// <summary>
    /// Gets the manifest diff between two versions of a game.
    /// </summary>
    /// <param name="game">The name of the game.</param>
    /// <param name="platform">The platform of the game.</param>
    /// <param name="oldRelease">The old release of the game.</param>
    /// <param name="oldVersion">The old version of the game.</param>
    /// <param name="newRelease">The new release of the game.</param>
    /// <param name="newVersion">The new version of the game.</param>
    /// <returns>The differences between the two versions as a string.</returns>
    Task<string> GetManifestDiffAsync(string game, string platform, string oldRelease, string oldVersion, string newRelease, string newVersion);

    /// <summary>
    /// Handles the event when a new Cytrus is detected.
    /// </summary>
    /// <param name="_">Ignored.</param>
    /// <param name="eventArgs">The event arguments.</param>
    ValueTask OnNewCytrusFileDetected(ICytrusWatcher _, NewCytrusFileDetectedEventArgs eventArgs);
}

public sealed class CytrusService : ICytrusService
{
    private readonly ICachedChannelsService _cachedChannelsService;
    private readonly ICytrusManifestFetcher _cytrusManifestFetcher;
    private readonly ICytrusWatcher _cytrusWatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="CytrusService"/> class.
    /// </summary>
    /// <param name="cachedChannelsService">The service to get the cached channels from.</param>
    /// <param name="cytrusManifestFetcher">The fetcher to get the manifest from.</param>
    /// <param name="cytrusWatcher">The watcher to get the Cytrus data from.</param>
    public CytrusService(ICachedChannelsService cachedChannelsService, ICytrusManifestFetcher cytrusManifestFetcher, ICytrusWatcher cytrusWatcher)
    {
        _cachedChannelsService = cachedChannelsService;
        _cytrusManifestFetcher = cytrusManifestFetcher;
        _cytrusWatcher = cytrusWatcher;
    }

    public async Task<string> GetManifestDiffAsync(string game, string platform, string oldRelease, string oldVersion, string newRelease, string newVersion)
    {
        var modelManifest = await _cytrusManifestFetcher.GetAsync(game, platform, oldRelease, oldVersion);
        if (modelManifest is null)
        {
            return "Old client not found.";
        }

        var currentManifest = await _cytrusManifestFetcher.GetAsync(game, platform, newRelease, newVersion);
        if (currentManifest is null)
        {
            return "New client not found.";
        }

        var diff = currentManifest.Value.Diff(modelManifest.Value);
        if (string.IsNullOrEmpty(diff))
        {
            return "No differences found.";
        }

        return diff;
    }

    public async ValueTask OnNewCytrusFileDetected(ICytrusWatcher _, NewCytrusFileDetectedEventArgs eventArgs)
    {
        await SendCytrusDiffAsync(eventArgs);
        await SendManifestDiffAsync(eventArgs);
    }

    /// <summary>
    /// Sends the Cytrus diff to the Cytrus channel.
    /// </summary>
    /// <param name="eventArgs">The event arguments containing the diff.</param>
    private async Task SendCytrusDiffAsync(NewCytrusFileDetectedEventArgs eventArgs)
    {
        var channel = _cachedChannelsService.CytrusChannel;
        if (channel is null)
        {
            return;
        }

        var content = Formatter.BlockCode(eventArgs.Diff, "json");

        if (content.Length > Constant.MaxMessageSize)
        {
            using MemoryStream stream = new(Encoding.UTF8.GetBytes(eventArgs.Diff));
            await channel.SendMessageSafeAsync("cytrus_diff.json", stream);
            return;
        }

        await channel.SendMessageSafeAsync(content);
    }

    /// <summary>
    /// Sends the manifest diff of the windows platform if it exists to the Cytrus manifest channel.
    /// </summary>
    /// <param name="eventArgs">The event arguments containing the diff.</param>
    private async Task SendManifestDiffAsync(NewCytrusFileDetectedEventArgs eventArgs)
    {
        var channel = _cachedChannelsService.CytrusManifestChannel;
        if (channel is null)
        {
            return;
        }

        var document = JsonDocument.Parse(eventArgs.Diff);
        var root = document.RootElement;

        if (!root.TryGetProperty("games", out var games) ||
            games.ValueKind != JsonValueKind.Object)
        {
            return;
        }

        foreach (var game in games.EnumerateObject())
        {
            if (!game.Value.TryGetProperty("platforms", out var platforms) ||
                platforms.ValueKind != JsonValueKind.Object)
            {
                return;
            }

            if (!platforms.TryGetProperty(CytrusGame.WindowsPlatform, out var platform) ||
                platform.ValueKind != JsonValueKind.Object)
            {
                return;
            }

            foreach (var release in platform.EnumerateObject())
            {
                if (!release.Value.TryGetProperty("-", out var oldRelease) ||
                    oldRelease.ValueKind != JsonValueKind.String)
                {
                    return;
                }

                if (!release.Value.TryGetProperty("+", out var newRelease) ||
                    newRelease.ValueKind != JsonValueKind.String)
                {
                    return;
                }

                var oldVersion = oldRelease.GetString();
                var newVersion = newRelease.GetString();
                if (string.IsNullOrEmpty(oldVersion) || string.IsNullOrEmpty(newVersion))
                {
                    return;
                }

                var mainContent = $"""
                     Diff of {Formatter.Bold(game.Name.Capitalize())} on {Formatter.Bold(CytrusGame.WindowsPlatform)}
                     {Formatter.InlineCode(oldVersion)} ({release.Name}) ➜ {Formatter.InlineCode(newVersion)} ({release.Name})
                     """;

                var diff = await GetManifestDiffAsync(game.Name, CytrusGame.WindowsPlatform, release.Name, oldVersion, release.Name, newVersion);

                if (mainContent.Length + diff.Length > Constant.MaxMessageSize)
                {
                    using MemoryStream stream = new(Encoding.UTF8.GetBytes(diff));

                    var message = new DiscordMessageBuilder()
                        .WithContent(mainContent)
                        .AddFile($"{game.Name}_{CytrusGame.WindowsPlatform}_{release.Name}_{oldVersion}_{release.Name}_{newVersion}.diff", stream);

                    await channel.SendMessageSafeAsync(message);
                    return;
                }

                await channel.SendMessageSafeAsync($"{mainContent}\n{Formatter.BlockCode(diff, "diff")}");
            }
        }
    }
}
