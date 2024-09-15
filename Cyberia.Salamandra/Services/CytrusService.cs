using Cyberia.Cytrusaurus;
using Cyberia.Cytrusaurus.EventArgs;
using Cyberia.Cytrusaurus.Extensions;
using Cyberia.Cytrusaurus.Models;
using Cyberia.Salamandra.Extensions.DSharpPlus;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

using System.Text;
using System.Text.Json;

namespace Cyberia.Salamandra.Services;

/// <summary>
/// Represents a service to handle the Cytrus events.
/// </summary>
public sealed class CytrusService
{
    private readonly CytrusManifestFetcher _cytrusManifestFetcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="CytrusService"/> class.
    /// </summary>
    /// <param name="cytrusManifestFetcher">The fetcher to get the manifest from.</param>
    public CytrusService(CytrusManifestFetcher cytrusManifestFetcher)
    {
        _cytrusManifestFetcher = cytrusManifestFetcher;
    }

    public async void OnNewCytrusDetected(object? _, NewCytrusFileDetectedEventArgs args)
    {
        await SendCytrusDiffAsync(args);
        await SendManifestDiffAsync(args);
    }

    /// <summary>
    /// Gets the manifest diff between two versions of a game.
    /// </summary>
    /// <param name="game">The name of the game.</param>
    /// <param name="platform">The platform of the game.</param>
    /// <param name="oldRelease">The old release of the game.</param>
    /// <param name="oldVersion">The old version of the game.</param>
    /// <param name="newRelease">The new release of the game.</param>
    /// <param name="newVersion">The new version of the game.</param>
    /// <returns>A string representing the differences between the two versions.</returns>
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

    /// <summary>
    /// Sends the Cytrus diff to the Cytrus channel.
    /// </summary>
    /// <param name="args">The event arguments containing the diff.</param>
    private static async Task SendCytrusDiffAsync(NewCytrusFileDetectedEventArgs args)
    {
        var channel = ChannelManager.CytrusChannel;
        if (channel is null)
        {
            return;
        }

        var content = Formatter.BlockCode(args.Diff, "json");

        if (content.Length > Constant.MaxMessageSize)
        {
            using MemoryStream stream = new(Encoding.UTF8.GetBytes(content));
            await channel.SendMessageSafeAsync("cytrus_diff.json", stream);
            return;
        }

        await channel.SendMessageSafeAsync(Formatter.BlockCode(args.Diff, "json"));
    }

    /// <summary>
    /// Sends the manifest diff of the windows platform to the Cytrus manifest channel.
    /// </summary>
    /// <param name="args">The event arguments containing the diff.</param>
    private async Task SendManifestDiffAsync(NewCytrusFileDetectedEventArgs args)
    {
        var channel = ChannelManager.CytrusManifestChannel;
        if (channel is null)
        {
            return;
        }

        var document = JsonDocument.Parse(args.Diff);
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
