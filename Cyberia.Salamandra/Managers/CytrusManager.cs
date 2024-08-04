using Cyberia.Cytrusaurus;
using Cyberia.Cytrusaurus.EventArgs;
using Cyberia.Cytrusaurus.Models;
using Cyberia.Salamandra.DsharpPlus;

using DSharpPlus;
using DSharpPlus.Entities;

using System.Text;
using System.Text.Json;

namespace Cyberia.Salamandra.Managers;

public static class CytrusManager
{
    public static async void OnNewCytrusDetected(object? _, NewCytrusDetectedEventArgs args)
    {
        await SendCytrusDiffAsync(args);
        await SendCytrusManifestDiffAsync(args);
    }

    public static async Task SendCytrusManifestDiffMessageAsync(this DiscordChannel channel, string game, string platform, string oldRelease, string oldVersion, string newRelease, string newVersion)
    {
        DiscordMessageBuilder messageBuilder = new();

        var modelManifest = await CytrusManifest.GetManifestAsync(game, platform, oldRelease, oldVersion);
        if (modelManifest is null)
        { 
            await channel.SendMessageSafe(messageBuilder.WithContent($"Old client not found."));
            return;
        }

        var currentManifest = await CytrusManifest.GetManifestAsync(game, platform, newRelease, newVersion);
        if (currentManifest is null)
        {
            await channel.SendMessageSafe(messageBuilder.WithContent($"New client not found."));
            return;
        }

        var diff = CytrusManifest.Diff(currentManifest.Value, modelManifest.Value);
        if (string.IsNullOrEmpty(diff))
        {
            diff = "No difference";
        }

        var mainContent = $"""
             Diff of {Formatter.Bold(game.Capitalize())} on {Formatter.Bold(platform)}
             {Formatter.InlineCode(oldVersion)} ({oldRelease}) ➜ {Formatter.InlineCode(newVersion)} ({newRelease})
             """;

        if (mainContent.Length + diff.Length < 2000)
        {
            await channel.SendMessageSafe(messageBuilder.WithContent($"{mainContent}\n{Formatter.BlockCode(diff, "diff")}"));
        }
        else
        {
            using MemoryStream stream = new(Encoding.UTF8.GetBytes(diff));
            await channel.SendMessageSafe(messageBuilder.WithContent(mainContent).AddFile($"{game}_{platform}_{oldRelease}_{oldVersion}_{newRelease}_{newVersion}.diff", stream));
        }
    }

    private static async Task SendCytrusDiffAsync(NewCytrusDetectedEventArgs args)
    {
        var channel = ChannelManager.CytrusChannel;
        if (channel is null)
        {
            return;
        }

        await channel.SendMessageAsync(new DiscordMessageBuilder().WithContent(Formatter.BlockCode(args.Diff, "json")));
    }

    private static async Task SendCytrusManifestDiffAsync(NewCytrusDetectedEventArgs args)
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

                await channel.SendCytrusManifestDiffMessageAsync(
                    game.Name,
                    CytrusGame.WindowsPlatform,
                    release.Name,
                    oldVersion,
                    release.Name,
                    newVersion);
            }
        }
    }
}
