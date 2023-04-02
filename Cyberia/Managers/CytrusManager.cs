using Cyberia.Cytrus;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

using System.Text.Json;

namespace Cyberia.Managers
{
    public static class CytrusManager
    {
        public static async void OnNewCytrusDetected(object? sender, NewCytrusDetectedEventArgs e)
        {
            //Send cytrus diff to discord
            DiscordChannel? channel = await Cyberia.Salamandra.Config.GetCytrusChannel();
            if (channel is null)
                Cyberia.Salamandra.Logger.Error($"Unknown cytrus channel (id:{Cyberia.Salamandra.Config.CytrusChannelId})");
            else
                await channel.SendMessageAsync(new DiscordMessageBuilder().WithContent(Formatter.BlockCode(e.Diff, "json")));

            //Send cytrus manifest diff of windows game to discord
            if (!Cyberia.Config.EnableAutomaticCytrusManifestDiff)
                return;

            channel = await Cyberia.Salamandra.Config.GetCytrusManifestDiffChannel();
            if (channel is null)
                Cyberia.Salamandra.Logger.Error($"Unknown cytrus manifest channel (id:{Cyberia.Salamandra.Config.CytrusManifestDiffChannelId})");
            else
            {
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
}
