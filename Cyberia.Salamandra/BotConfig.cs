using DSharpPlus.Entities;

namespace Cyberia.Salamandra
{
    public sealed class BotConfig
    {
        private const string PATH = $"bot.config.json";

        public string Token { get; init; }
        public string CdnUrl { get; init; }
        public string EmbedColor { get; init; }
        public string DiscordInviteUrl { get; init; }
        public ulong LogChannelId { get; init; }
        public ulong CommandErrorChannelId { get; init; }
        public ulong LangChannelId { get; init; }
        public ulong CytrusChannelId { get; init; }
        public ulong CytrusManifestDiffChannelId { get; init; }

        public BotConfig()
        {
            Token = string.Empty;
            CdnUrl = string.Empty;
            EmbedColor = string.Empty;
            DiscordInviteUrl = string.Empty;
        }

        public static BotConfig Build()
        {
            if (!File.Exists(PATH))
            {
                Bot.Instance.Logger.Crit($"Configuration file not found at {PATH}");
                Console.ReadLine();
                Environment.Exit(0);
            }

            return Json.LoadFromFile<BotConfig>(PATH);
        }

        public async Task<DiscordChannel?> GetLogChannel()
        {
            try
            {
                return await Bot.Instance.Client.GetChannelAsync(LogChannelId);
            }
            catch
            {
                return null;
            }
        }

        public async Task<DiscordChannel?> GetCommandErrorChannel()
        {
            try
            {
                return await Bot.Instance.Client.GetChannelAsync(CommandErrorChannelId);
            }
            catch
            {
                return null;
            }
        }

        public async Task<DiscordChannel?> GetLangChannel()
        {
            try
            {
                return await Bot.Instance.Client.GetChannelAsync(LangChannelId);
            }
            catch
            {
                return null;
            }
        }

        public async Task<DiscordChannel?> GetCytrusChannel()
        {
            try
            {
                return await Bot.Instance.Client.GetChannelAsync(CytrusChannelId);
            }
            catch
            {
                return null;
            }
        }

        public async Task<DiscordChannel?> GetCytrusManifestDiffChannel()
        {
            try
            {
                return await Bot.Instance.Client.GetChannelAsync(CytrusManifestDiffChannelId);
            }
            catch
            {
                return null;
            }
        }
    }
}
