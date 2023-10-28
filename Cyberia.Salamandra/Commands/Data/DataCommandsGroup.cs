namespace Cyberia.Salamandra.Commands.Data
{
    public static class DataCommandsGroup
    {
        public static void Register(params ulong[] guildsId)
        {
            if (guildsId.Length == 0)
            {
                throw new ArgumentException("We need at least one guild id to register to");
            }

            foreach (ulong guildId in guildsId)
            {
                Bot.SlashCommands.RegisterCommands<CytrusCommandModule>(guildId);
                Bot.SlashCommands.RegisterCommands<LangsCommandModule>(guildId);
                Bot.SlashCommands.RegisterCommands<ReloadCommandModule>(guildId);
            }
        }
    }
}
