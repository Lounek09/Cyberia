using Cyberia.Cytrusaurus;

using DSharpPlus;
using DSharpPlus.Entities;

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

        private static async Task SendCytrusDiffAsync(NewCytrusDetectedEventArgs e)
        {
            DiscordChannel? channel = await Bot.Instance.Config.GetCytrusChannel();
            if (channel is null)
            {
                Bot.Instance.Logger.Error($"Unknown cytrus channel (id:{Bot.Instance.Config.CytrusChannelId})");
                return;
            }

            await channel.SendMessageAsync(new DiscordMessageBuilder().WithContent(Formatter.BlockCode(e.Diff, "json")));
        }

        private static async Task SendCytrusManifestDiffAsync(NewCytrusDetectedEventArgs e)
        {
            DiscordChannel? channel = await Bot.Instance.Config.GetCytrusManifestDiffChannel();
            if (channel is null)
            {
                Bot.Instance.Logger.Error($"Unknown cytrus manifest channel (id:{Bot.Instance.Config.CytrusManifestDiffChannelId})");
                return;
            }

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
