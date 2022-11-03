namespace Salamandra.Bot.Commands.Data
{
    public static class DataCommandsGroup
    {
        public static void Register(params ulong[] guildsId)
        {
            if (guildsId.Length == 0)
                throw new ArgumentException("We need at least one guild id to register to");

            foreach (ulong guildId in guildsId)
            {
                DiscordBot.Instance.SlashCommands.RegisterCommands<CytrusCommandModule>(guildId);
                DiscordBot.Instance.SlashCommands.RegisterCommands<LangsCommandModule>(guildId);
                DiscordBot.Instance.SlashCommands.RegisterCommands<ReloadCommandModule>(guildId);
            }
        }
    }
}
