using DSharpPlus;
using DSharpPlus.Entities;

using Salamandra.Bot.Managers;
using Salamandra.Cytrus;

using System.Text.Json;

namespace Salamandra.Managers
{
    public static class CytrusManager
    {
        private static Dictionary<string, Timer> _timer = new();

        public static void Listen()
        {
            _timer = new();

            if (Salamandra.Config.EnableCheckCytrus)
                _timer.Add("Cytrus", new Timer(async _ => await Salamandra.Cytrus.Launch(), null, 10000, 60000));
        }

        public static async void OnNewCytrusDetected(object? sender, NewCytrusDetectedEventArgs e)
        {
            DiscordChannel? channel = await Salamandra.Bot.Config.GetCytrusChannel();
            if (channel is not null)
                await channel.SendMessage(new DiscordMessageBuilder().WithContent(Formatter.BlockCode(e.Diff, "json")));

            channel = await Salamandra.Bot.Config.GetCytrusManifestDiffChannel();
            if (Salamandra.Config.EnableAutomaticCytrusManifestDiff && channel is not null)
            {
                JsonDocument document = JsonDocument.Parse(e.Diff);
                JsonElement root = document.RootElement;

                if (!root.TryGetProperty("games", out JsonElement games))
                    return;

                foreach (JsonProperty game in games.EnumerateObject())
                {
                    if (!game.Value.TryGetProperty("platforms", out JsonElement platforms))
                        return;

                    foreach (JsonProperty platform in platforms.EnumerateObject())
                    {
                        foreach (JsonProperty release in platform.Value.EnumerateObject())
                        {
                            if (!release.Value.TryGetProperty("-", out JsonElement minus))
                                return;

                            if (!release.Value.TryGetProperty("+", out JsonElement plus))
                                return;

                            string? oldVersion = minus.GetString();
                            string? newVersion = plus.GetString();

                            if (string.IsNullOrEmpty(oldVersion) || string.IsNullOrEmpty(newVersion))
                                return;

                            await channel.SendCytrusManifestDiffMessage(game.Name, platform.Name, release.Name, oldVersion, release.Name, newVersion);
                        }
                    }
                }
            }
        }
    }
}
