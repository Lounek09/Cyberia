namespace Cyberia.Salamandra.Commands.Data
{
    public static class DataCommandsGroup
    {
        public static void Register(params ulong[] guildsId)
        {
            if (guildsId.Length == 0)
                throw new ArgumentException("We need at least one guild id to register to");

            foreach (ulong guildId in guildsId)
            {
                Bot.Instance.SlashCommands.RegisterCommands<CytrusCommandModule>(guildId);
                Bot.Instance.SlashCommands.RegisterCommands<LangsCommandModule>(guildId);
                Bot.Instance.SlashCommands.RegisterCommands<ReloadCommandModule>(guildId);
            }
        }
    }
}
