namespace Cyberia.Salamandra.Commands.Admin
{
    public static class AdminCommandsGroup
    {
        public static void Register(params ulong[] guildsId)
        {
            if (guildsId.Length == 0)
            {
                throw new ArgumentException("We need at least one guild id to register to");
            }

            foreach (ulong guildId in guildsId)
            {
                Bot.SlashCommands.RegisterCommands<IpCommandModule>(guildId);
                Bot.SlashCommands.RegisterCommands<KillCommandModule>(guildId);
                Bot.SlashCommands.RegisterCommands<LeaveCommandModule>(guildId);
                Bot.SlashCommands.RegisterCommands<ParseCommandModule>(guildId);
                Bot.SlashCommands.RegisterCommands<RestartCommandModule>(guildId);
                Bot.SlashCommands.RegisterCommands<TestCommandModule>(guildId);
            }
        }
    }
}
