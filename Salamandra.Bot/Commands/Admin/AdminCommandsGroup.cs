namespace Salamandra.Bot.Commands.Admin
{
    public static class AdminCommandsGroup
    {
        public static void Register(params ulong[] guildsId)
        {
            if (guildsId.Length == 0)
                throw new ArgumentException("We need at least one guild id to register to");

            foreach (ulong guildId in guildsId)
            {
                DiscordBot.Instance.SlashCommands.RegisterCommands<IpCommandModule>(guildId);
                DiscordBot.Instance.SlashCommands.RegisterCommands<KillCommandModule>(guildId);
                DiscordBot.Instance.SlashCommands.RegisterCommands<LeaveCommandModule>(guildId);
                DiscordBot.Instance.SlashCommands.RegisterCommands<RestartCommandModule>(guildId);
                DiscordBot.Instance.SlashCommands.RegisterCommands<TestCommandModule>(guildId);
            }
        }
    }
}
