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
    /// <param name="currentRelease">The current release of the game to diff.</param>
    /// <param name="currentVersion">The current version of the game to diff.</param>
    /// <param name="modelRelease">The model release of the game.</param>
    /// <param name="modelVersion">The model version of the game.</param>
    /// <returns>A string representing the difference between the versions.</returns>
    Task<string> GetManifestDiffAsync(string game, string platform, string currentRelease, string currentVersion, string modelRelease, string modelVersion);

    /// <summary>
    /// Handles the event when a new Cytrus is detected.
    /// </summary>
    /// <param name="_">Ignored.</param>
    /// <param name="eventArgs">The event arguments.</param>
    ValueTask OnNewCytrusDetected(ICytrusWatcher _, NewCytrusDetectedEventArgs eventArgs);

    /// <summary>
    /// Handles the event when an error occurs in the Cytrus watcher.
    /// </summary>
    /// <param name="_">Ignored.</param>
    /// <param name="eventArgs">The event arguments.</param>
    ValueTask OnCytrusErrored(ICytrusWatcher _, CytrusErroredEventArgs eventArgs);
}

public sealed class CytrusService : ICytrusService
{
    private readonly ICachedChannelsManager _cachedChannelsManager;
    private readonly ICytrusManifestFetcher _cytrusManifestFetcher;
    private readonly IEmbedBuilderService _embedBuilderService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CytrusService"/> class.
    /// </summary>
    public CytrusService(ICachedChannelsManager cachedChannelsManager, ICytrusManifestFetcher cytrusManifestFetcher, IEmbedBuilderService embedBuilderService)
    {
        _cachedChannelsManager = cachedChannelsManager;
        _cytrusManifestFetcher = cytrusManifestFetcher;
        _embedBuilderService = embedBuilderService;
    }

    public async Task<string> GetManifestDiffAsync(string game, string platform, string currentRelease, string currentVersion, string modelRelease, string modelVersion)
    {
        var currentManifest = await _cytrusManifestFetcher.GetAsync(game, platform, currentRelease, currentVersion);
        if (currentManifest is null)
        {
            return "Current client not found.";
        }

        var modelManifest = await _cytrusManifestFetcher.GetAsync(game, platform, modelRelease, modelVersion);
        if (modelManifest is null)
        {
            return "Model client not found.";
        }

        var diff = currentManifest.Value.Diff(modelManifest.Value);
        if (string.IsNullOrEmpty(diff))
        {
            return "No differences found.";
        }

        return diff;
    }

    public async ValueTask OnNewCytrusDetected(ICytrusWatcher _, NewCytrusDetectedEventArgs eventArgs)
    {
        await SendCytrusDiffAsync(eventArgs);
        await SendManifestDiffAsync(eventArgs);
    }

    public async ValueTask OnCytrusErrored(ICytrusWatcher _, CytrusErroredEventArgs eventArgs)
    {
        var embed = _embedBuilderService.CreateErrorEmbedBuilder(
            "An error occurred while checking for a new Cytrus version",
            eventArgs.ErrorMessage,
            eventArgs.Exception);

        await _cachedChannelsManager.SendErrorMessage(embed);
    }

    /// <summary>
    /// Sends the Cytrus diff to the Cytrus channel.
    /// </summary>
    /// <param name="eventArgs">The event arguments containing the diff.</param>
    private async Task SendCytrusDiffAsync(NewCytrusDetectedEventArgs eventArgs)
    {
        var channel = _cachedChannelsManager.CytrusChannel;
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
    private async Task SendManifestDiffAsync(NewCytrusDetectedEventArgs eventArgs)
    {
        var channel = _cachedChannelsManager.CytrusManifestChannel;
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

                var mainContent =
                $"""
                Diff of {Formatter.Bold(game.Name.Capitalize())} on {Formatter.Bold(CytrusGame.WindowsPlatform)}
                {Formatter.InlineCode(newVersion)} ({release.Name}) compared to {Formatter.InlineCode(oldVersion)} ({release.Name})
                """;

                var diff = await GetManifestDiffAsync(game.Name, CytrusGame.WindowsPlatform, release.Name, newVersion, release.Name, oldVersion);

                if (mainContent.Length + diff.Length + 10 > Constant.MaxMessageSize) // 10 for the block code formatting
                {
                    using MemoryStream stream = new(Encoding.UTF8.GetBytes(diff));

                    var message = new DiscordMessageBuilder()
                        .WithContent(mainContent)
                        .AddFile($"{game.Name}_{CytrusGame.WindowsPlatform}_{release.Name}_{newVersion}_{oldVersion}.diff", stream);

                    await channel.SendMessageSafeAsync(message);
                    return;
                }

                await channel.SendMessageSafeAsync($"{mainContent}\n{Formatter.BlockCode(diff, "diff")}");
            }
        }
    }
}
