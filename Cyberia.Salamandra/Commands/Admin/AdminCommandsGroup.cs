using DSharpPlus.Commands;

namespace Cyberia.Salamandra.Commands.Admin;

public static class AdminCommandsGroup
{
    public static void RegisterAdminCommands(this CommandsExtension extension, ulong guildId)
    {
        extension.AddCommand(IpCommandModule.ExecuteAsync);
        extension.AddCommand(KillCommandModule.ExecuteAsync);
        extension.AddCommand(LeaveCommandModule.ExecuteAsync);
        extension.AddCommands<ParseCommandModule>();
        extension.AddCommand(RestartCommandModule.ExecuteAsync);
        extension.AddCommands<SearchCommandModule>();
        extension.AddCommand(TestCommandModule.ExecuteAsync);
    }
}
