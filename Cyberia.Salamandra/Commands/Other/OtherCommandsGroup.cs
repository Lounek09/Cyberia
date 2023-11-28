namespace Cyberia.Salamandra.Commands.Other;

public static class OtherCommandsGroup
{
    public static void Register()
    {
        Bot.SlashCommands.RegisterCommands<DiscordCommandModule>();
        Bot.SlashCommands.RegisterCommands<HelpCommandModule>();
        Bot.SlashCommands.RegisterCommands<PingCommandModule>();
    }
}
