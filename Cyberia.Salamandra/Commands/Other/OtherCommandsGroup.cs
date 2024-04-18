using DSharpPlus.Commands;

namespace Cyberia.Salamandra.Commands.Other;

public static class OtherCommandsGroup
{
    public static void RegisterOtherCommands(this CommandsExtension extension)
    {
        extension.AddCommand(DiscordCommandModule.ExecuteAsync);
        extension.AddCommand(HelpCommandModule.ExecuteAsync);
        extension.AddCommand(PingCommandModule.ExecuteAsync);
    }
}
