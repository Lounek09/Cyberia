using DSharpPlus.Commands;

namespace Cyberia.Salamandra.Commands.Data;

public static class DataCommandsGroup
{
    public static void RegisterDataCommands(this CommandsExtension extension, ulong guildId)
    {
        extension.AddCommands<CytrusCommandModule>();
        extension.AddCommands<LangsCommandModule>();
        extension.AddCommand(ReloadCommandModule.ExecuteAsync);
    }
}
