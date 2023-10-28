namespace Cyberia.Salamandra.Commands.Admin
{
    public static class AdminCommandsGroup
    {
        public static void Register(ulong guildId)
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
