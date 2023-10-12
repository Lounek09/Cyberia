using Cyberia.Cytrusaurus;
using Cyberia.Cytrusaurus.Models;
using Cyberia.Cytrusaurus.Models.FlatBuffers;

using DSharpPlus;
using DSharpPlus.Entities;

using Google.FlatBuffers;

using System.Text;
using System.Text.Json;

namespace Cyberia.Salamandra.Managers
{
    public static class CytrusManager
    {
        public static async void OnNewCytrusDetected(object? sender, NewCytrusDetectedEventArgs e)
        {
            await SendCytrusDiffAsync(e);
            await SendCytrusManifestDiffAsync(e);
        }

        public static async Task SendCytrusManifestDiffMessageAsync(this DiscordChannel channel, string game, string platform, string oldRelease, string oldVersion, string newRelease, string newVersion)
        {
            HttpClient httpClient = new();
            DiscordMessageBuilder message = new();

            string url1 = CytrusData.GetGameManifestUrl(game, platform, oldRelease, oldVersion);
            Manifest client1;
            try
            {
                byte[] metafile = await httpClient.GetByteArrayAsync(url1);
                ByteBuffer buffer = new(metafile);
                client1 = Manifest.GetRootAsManifest(buffer);
            }
            catch (HttpRequestException)
            {
                await channel.SendMessage(message.WithContent($"Nouveau client introuvable"));
                return;
            }

            string url2 = CytrusData.GetGameManifestUrl(game, platform, newRelease, newVersion);
            Manifest client2;
            try
            {
                byte[] metafile = await httpClient.GetByteArrayAsync(url2);
                ByteBuffer buffer = new(metafile);
                client2 = Manifest.GetRootAsManifest(buffer);
            }
            catch (HttpRequestException)
            {
                await channel.SendMessage(message.WithContent($"Nouveau client introuvable"));
                return;
            }

            string diff = client2.Diff(client1);

            string mainContent = $"""
                 Diff de {Formatter.Bold(game.Capitalize())} sur {Formatter.Bold(platform)}
                 {Formatter.InlineCode(oldVersion)} ({oldRelease}) ➜ {Formatter.InlineCode(newVersion)} ({newRelease})
                 """;

            if (mainContent.Length + diff.Length < 2000)
                await channel.SendMessage(message.WithContent($"{mainContent}\n{Formatter.BlockCode(diff, "diff")}"));
            else
            {
                using MemoryStream stream = new(Encoding.UTF8.GetBytes(diff));
                await channel.SendMessage(message.WithContent(mainContent).AddFile($"{game}_{platform}_{oldRelease}_{oldVersion}_{newRelease}_{newVersion}.diff", stream));
            }
        }

        private static async Task<DiscordChannel?> GetCytrusChannel()
        {
            ulong id = Bot.Instance.Config.CytrusChannelId;
            if (id == 0)
                return null;

            try
            {
                return await Bot.Instance.Client.GetChannelAsync(id);
            }
            catch
            {
                Log.Error("Unknown cytrus channel (id:{id})", id);
                return null;
            }
        }

        private static async Task<DiscordChannel?> GetCytrusManifestChannel()
        {
            ulong id = Bot.Instance.Config.CytrusManifestChannelId;
            if (id == 0)
                return null;

            try
            {
                return await Bot.Instance.Client.GetChannelAsync(id);
            }
            catch
            {
                Log.Error("Unknown cytrus manifest channel (id:{id})", id);
                return null;
            }
        }

        private static async Task SendCytrusDiffAsync(NewCytrusDetectedEventArgs e)
        {
            DiscordChannel? channel = await GetCytrusChannel();
            if (channel is null)
                return;

            await channel.SendMessageAsync(new DiscordMessageBuilder().WithContent(Formatter.BlockCode(e.Diff, "json")));
        }

        private static async Task SendCytrusManifestDiffAsync(NewCytrusDetectedEventArgs e)
        {
            DiscordChannel? channel = await GetCytrusManifestChannel();
            if (channel is null)
                return;

            JsonDocument document = JsonDocument.Parse(e.Diff);
            JsonElement root = document.RootElement;

            if (!root.TryGetProperty("games", out JsonElement games))
                return;

            foreach (JsonProperty game in games.EnumerateObject())
            {
                if (!game.Value.TryGetProperty("platforms", out JsonElement platforms))
                    return;

                if (!platforms.TryGetProperty("windows", out JsonElement platform))
                    return;

                foreach (JsonProperty release in platform.EnumerateObject())
                {
                    if (!release.Value.TryGetProperty("-", out JsonElement minus))
                        return;

                    if (!release.Value.TryGetProperty("+", out JsonElement plus))
                        return;

                    string? oldVersion = minus.GetString();
                    string? newVersion = plus.GetString();

                    if (string.IsNullOrEmpty(oldVersion) || string.IsNullOrEmpty(newVersion))
                        return;

                    await channel.SendCytrusManifestDiffMessageAsync(game.Name, "windows", release.Name, oldVersion, release.Name, newVersion);
                }
            }
        }
    }
}
