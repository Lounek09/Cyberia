using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Data;

public static class DataCommandsGroup
{
    public static void RegisterDataCommands(this SlashCommandsExtension extension, ulong guildId)
    {
        extension.RegisterCommands<CytrusCommandModule>(guildId);
        extension.RegisterCommands<LangsCommandModule>(guildId);
        extension.RegisterCommands<ReloadCommandModule>(guildId);
    }
}
