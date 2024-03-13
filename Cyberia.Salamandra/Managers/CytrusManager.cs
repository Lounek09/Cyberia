using Cyberia.Cytrusaurus.EventArgs;
using Cyberia.Cytrusaurus.Models;
using Cyberia.Cytrusaurus.Models.FlatBuffers;

using DSharpPlus;
using DSharpPlus.Entities;

using Google.FlatBuffers;

using System.Text;
using System.Text.Json;

namespace Cyberia.Salamandra.Managers;

public static class CytrusManager
{
    public static async void OnNewCytrusDetected(object? sender, NewCytrusDetectedEventArgs e)
    {
        await SendCytrusDiffAsync(e);
        await SendCytrusManifestDiffAsync(e);
    }

    public static async Task SendCytrusManifestDiffMessageAsync(this DiscordChannel channel, string game, string platform, string oldRelease, string oldVersion, string newRelease, string newVersion)
    {
        var httpClient = new HttpClient();
        var message = new DiscordMessageBuilder();

        var url1 = CytrusData.GetGameManifestUrl(game, platform, oldRelease, oldVersion);
        Manifest client1;
        try
        {
            var metafile = await httpClient.GetByteArrayAsync(url1);
            var buffer = new ByteBuffer(metafile);
            client1 = Manifest.GetRootAsManifest(buffer);
        }
        catch (HttpRequestException)
        {
            await channel.SendMessage(message.WithContent($"Nouveau client introuvable"));
            return;
        }

        var url2 = CytrusData.GetGameManifestUrl(game, platform, newRelease, newVersion);
        Manifest client2;
        try
        {
            var metafile = await httpClient.GetByteArrayAsync(url2);
            var buffer = new ByteBuffer(metafile);
            client2 = Manifest.GetRootAsManifest(buffer);
        }
        catch (HttpRequestException)
        {
            await channel.SendMessage(message.WithContent($"Nouveau client introuvable"));
            return;
        }

        var diff = client2.Diff(client1);

        var mainContent = $"""
             Diff de {Formatter.Bold(game.Capitalize())} sur {Formatter.Bold(platform)}
             {Formatter.InlineCode(oldVersion)} ({oldRelease}) ➜ {Formatter.InlineCode(newVersion)} ({newRelease})
             """;

        if (mainContent.Length + diff.Length < 2000)
        {
            await channel.SendMessage(message.WithContent($"{mainContent}\n{Formatter.BlockCode(diff, "diff")}"));
        }
        else
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(diff));
            await channel.SendMessage(message.WithContent(mainContent).AddFile($"{game}_{platform}_{oldRelease}_{oldVersion}_{newRelease}_{newVersion}.diff", stream));
        }
    }

    private static async Task<DiscordChannel?> GetCytrusChannel()
    {
        var id = Bot.Config.CytrusChannelId;
        if (id == 0)
        {
            return null;
        }

        try
        {
            return await Bot.Client.GetChannelAsync(id);
        }
        catch
        {
            Log.Error("Unknown cytrus channel {ChannelId}", id);
            return null;
        }
    }

    private static async Task<DiscordChannel?> GetCytrusManifestChannel()
    {
        var id = Bot.Config.CytrusManifestChannelId;
        if (id == 0)
        {
            return null;
        }

        try
        {
            return await Bot.Client.GetChannelAsync(id);
        }
        catch
        {
            Log.Error("Unknown cytrus manifest channel {ChannelId}", id);
            return null;
        }
    }

    private static async Task SendCytrusDiffAsync(NewCytrusDetectedEventArgs e)
    {
        var channel = await GetCytrusChannel();
        if (channel is null)
        {
            return;
        }

        await channel.SendMessageAsync(new DiscordMessageBuilder().WithContent(Formatter.BlockCode(e.Diff, "json")));
    }

    private static async Task SendCytrusManifestDiffAsync(NewCytrusDetectedEventArgs e)
    {
        var channel = await GetCytrusManifestChannel();
        if (channel is null)
        {
            return;
        }

        var document = JsonDocument.Parse(e.Diff);
        var root = document.RootElement;

        if (!root.TryGetProperty("games", out var games))
        {
            return;
        }

        foreach (var game in games.EnumerateObject())
        {
            if (!game.Value.TryGetProperty("platforms", out var platforms))
            {
                return;
            }

            if (!platforms.TryGetProperty("windows", out var platform))
            {
                return;
            }

            foreach (var release in platform.EnumerateObject())
            {
                if (!release.Value.TryGetProperty("-", out var minus))
                {
                    return;
                }

                if (!release.Value.TryGetProperty("+", out var plus))
                {
                    return;
                }

                var oldVersion = minus.GetString();
                var newVersion = plus.GetString();

                if (string.IsNullOrEmpty(oldVersion) || string.IsNullOrEmpty(newVersion))
                {
                    return;
                }

                await channel.SendCytrusManifestDiffMessageAsync(game.Name, "windows", release.Name, oldVersion, release.Name, newVersion);
            }
        }
    }
}
