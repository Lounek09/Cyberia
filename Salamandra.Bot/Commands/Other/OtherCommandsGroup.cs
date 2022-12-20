namespace Salamandra.Bot.Commands.Other
{
    public static class OtherCommandsGroup
    {
        public static void Register()
        {
            DiscordBot.Instance.SlashCommands.RegisterCommands<DiscordCommandModule>();
            DiscordBot.Instance.SlashCommands.RegisterCommands<PingCommandModule>();
        }
    }
}
