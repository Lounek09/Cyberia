using DSharpPlus.Commands;

namespace Cyberia.Salamandra.Commands.Admin;

public static class AdminCommandsGroup
{
    public static void RegisterAdminCommands(this CommandsExtension extension, ulong guildId)
    {
        extension.AddCommand(IpCommandModule.ExecuteAsync, guildId);
        extension.AddCommand(KillCommandModule.ExecuteAsync, guildId);
        extension.AddCommand(LeaveCommandModule.ExecuteAsync, guildId);
        extension.AddCommands<ParseCommandModule>(guildId);
        extension.AddCommand(RestartCommandModule.ExecuteAsync, guildId);
        extension.AddCommands<SearchCommandModule>(guildId);
        extension.AddCommand(TestCommandModule.ExecuteAsync, guildId);
    }
}
