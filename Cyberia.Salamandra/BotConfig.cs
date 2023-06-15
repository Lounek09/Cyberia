using DSharpPlus.Entities;

namespace Cyberia.Salamandra
{
    public sealed class BotConfig
    {
        private const string PATH = $"bot.config.json";

        public string Token { get; init; }
        public string EmbedColor { get; init; }
        public string DiscordInviteUrl { get; init; }
        public ulong LogChannelId { get; init; }
        public ulong CommandErrorChannelId { get; init; }
        public ulong LangForumChannelId { get; init; }
        public ulong CytrusChannelId { get; init; }
        public ulong CytrusManifestDiffChannelId { get; init; }

        public BotConfig()
        {
            Token = string.Empty;
            EmbedColor = string.Empty;
            DiscordInviteUrl = string.Empty;
        }

        public static BotConfig Load()
        {
            if (!File.Exists(PATH))
            {
                Console.WriteLine($"Configuration file not found at {PATH}");
                Console.ReadLine();
                Environment.Exit(0);
            }

            return Json.LoadFromFile<BotConfig>(PATH);
        }
    }
}
