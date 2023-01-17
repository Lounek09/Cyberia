using DSharpPlus.Entities;

namespace Salamandra.Bot
{
    public sealed class BotConfig
    {
        public string Token { get; set; }
        public string CdnUrl { get; set; }
        public string EmbedColor { get; set; }
        public string DiscordInviteUrl { get; set; }
        public ulong LogChannelId { get; set; }
        public ulong CommandErrorChannelId { get; set; }
        public ulong LangChannelId { get; set; }
        public ulong CytrusChannelId { get; set; }
        public ulong CytrusManifestDiffChannelId { get; set; }

        public BotConfig()
        {
            Token = string.Empty;
            CdnUrl = string.Empty;
            EmbedColor = string.Empty;
            DiscordInviteUrl = string.Empty;
        }

        public static BotConfig Build()
        {
            if (!File.Exists(Constant.CONFIG_PATH))
            {
                DiscordBot.Instance.Logger.Crit($"Configuration file not found at {Constant.CONFIG_PATH}");
                Console.ReadLine();
                Environment.Exit(0);
            }

            return Json.LoadFromFile<BotConfig>(Constant.CONFIG_PATH);
        }

        public async Task<DiscordChannel?> GetLogChannel()
        {
            try
            {
                return await DiscordBot.Instance.Client.GetChannelAsync(LogChannelId);
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
                return await DiscordBot.Instance.Client.GetChannelAsync(CommandErrorChannelId);
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
                return await DiscordBot.Instance.Client.GetChannelAsync(LangChannelId);
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
                return await DiscordBot.Instance.Client.GetChannelAsync(CytrusChannelId);
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
                return await DiscordBot.Instance.Client.GetChannelAsync(CytrusManifestDiffChannelId);
            }
            catch
            {
                return null;
            }
        }
    }
}
