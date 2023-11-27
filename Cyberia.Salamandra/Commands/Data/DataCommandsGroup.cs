namespace Cyberia.Salamandra.Commands.Data;

public static class DataCommandsGroup
{
    public static void Register(ulong guildId)
    {
        Bot.SlashCommands.RegisterCommands<CytrusCommandModule>(guildId);
        Bot.SlashCommands.RegisterCommands<LangsCommandModule>(guildId);
        Bot.SlashCommands.RegisterCommands<ReloadCommandModule>(guildId);
    }
}
