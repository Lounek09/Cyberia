namespace Cyberia.Salamandra.Commands.Dofus
{
    public static class DofusCommandsGroup
    {
        public static void Register()
        {
            Bot.SlashCommands.RegisterCommands<BreedCommandModule>();
            Bot.SlashCommands.RegisterCommands<CraftCommandModule>();
            Bot.SlashCommands.RegisterCommands<CritCommandModule>();
            Bot.SlashCommands.RegisterCommands<EscapeCommandModule>();
            Bot.SlashCommands.RegisterCommands<HouseCommandModule>();
            Bot.SlashCommands.RegisterCommands<IncarnationCommandModule>();
            Bot.SlashCommands.RegisterCommands<ItemCommandModule>();
            Bot.SlashCommands.RegisterCommands<ItemSetCommandModule>();
            Bot.SlashCommands.RegisterCommands<MapCommandModule>();
            Bot.SlashCommands.RegisterCommands<MonsterCommandModule>();
            Bot.SlashCommands.RegisterCommands<QuestCommandModule>();
            Bot.SlashCommands.RegisterCommands<RuneCommandModule>();
            Bot.SlashCommands.RegisterCommands<SpellCommandModule>();
        }
    }
}
