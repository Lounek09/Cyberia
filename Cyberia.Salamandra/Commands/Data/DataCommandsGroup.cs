using DSharpPlus.Commands;

namespace Cyberia.Salamandra.Commands.Data;

public static class DataCommandsGroup
{
    public static void RegisterDataCommands(this CommandsExtension extension, ulong guildId)
    {
        extension.AddCommands<CytrusCommandModule>(guildId);
        extension.AddCommands<LangsCommandModule>(guildId);
        extension.AddCommand(ReloadCommandModule.ExecuteAsync, guildId);
    }
}
