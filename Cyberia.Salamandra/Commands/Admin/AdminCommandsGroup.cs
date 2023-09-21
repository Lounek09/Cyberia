namespace Cyberia.Salamandra.Commands.Admin
{
    public static class AdminCommandsGroup
    {
        public static void Register(params ulong[] guildsId)
        {
            if (guildsId.Length == 0)
                throw new ArgumentException("We need at least one guild id to register to");

            foreach (ulong guildId in guildsId)
            {
                Bot.Instance.SlashCommands.RegisterCommands<IpCommandModule>(guildId);
                Bot.Instance.SlashCommands.RegisterCommands<KillCommandModule>(guildId);
                Bot.Instance.SlashCommands.RegisterCommands<LeaveCommandModule>(guildId);
                Bot.Instance.SlashCommands.RegisterCommands<ParseCommandModule>(guildId);
                Bot.Instance.SlashCommands.RegisterCommands<RestartCommandModule>(guildId);
                Bot.Instance.SlashCommands.RegisterCommands<TestCommandModule>(guildId);
            }
        }
    }
}
