namespace Cyberia.Salamandra
{
    public sealed class BotConfig
    {
        public string Token { get; init; }
        public string EmbedColor { get; init; }
        public ulong AdminGuildId { get; init; }
        public string DiscordGuildInviteUrl { get; init; }
        public ulong LogChannelId { get; init; }
        public ulong CommandErrorChannelId { get; init; }
        public ulong LangForumChannelId { get; init; }
        public ulong CytrusChannelId { get; init; }
        public ulong CytrusManifestChannelId { get; init; }

        public BotConfig()
        {
            Token = string.Empty;
            EmbedColor = string.Empty;
            DiscordGuildInviteUrl = string.Empty;
        }
    }
}
