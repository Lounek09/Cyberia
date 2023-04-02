namespace Cyberia.Salamandra.Commands.Other
{
    public static class OtherCommandsGroup
    {
        public static void Register()
        {
            Bot.Instance.SlashCommands.RegisterCommands<DiscordCommandModule>();
            Bot.Instance.SlashCommands.RegisterCommands<PingCommandModule>();
        }
    }
}
