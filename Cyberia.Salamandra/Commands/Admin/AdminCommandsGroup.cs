using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Admin;

public static class AdminCommandsGroup
{
    public static void RegisterAdminCommands(this SlashCommandsExtension extension, ulong guildId)
    {
        extension.RegisterCommands<IpCommandModule>(guildId);
        extension.RegisterCommands<KillCommandModule>(guildId);
        extension.RegisterCommands<LeaveCommandModule>(guildId);
        extension.RegisterCommands<ParseCommandModule>(guildId);
        extension.RegisterCommands<RestartCommandModule>(guildId);
        extension.RegisterCommands<SearchCommandModule>(guildId);
        extension.RegisterCommands<TestCommandModule>(guildId);
    }
}
