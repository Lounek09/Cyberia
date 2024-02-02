using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Other;

public static class OtherCommandsGroup
{
    public static void RegisterOtherCommands(this SlashCommandsExtension extension)
    {
        extension.RegisterCommands<DiscordCommandModule>();
        extension.RegisterCommands<HelpCommandModule>();
        extension.RegisterCommands<PingCommandModule>();
    }
}
