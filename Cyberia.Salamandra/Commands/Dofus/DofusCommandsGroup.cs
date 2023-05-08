namespace Cyberia.Salamandra.Commands.Dofus
{
    public static class DofusCommandsGroup
    {
        public static void Register()
        {
            Bot.Instance.SlashCommands.RegisterCommands<BreedCommandModule>();
            Bot.Instance.SlashCommands.RegisterCommands<CraftCommandModule>();
            Bot.Instance.SlashCommands.RegisterCommands<CritCommandModule>();
            Bot.Instance.SlashCommands.RegisterCommands<EscapeCommandModule>();
            Bot.Instance.SlashCommands.RegisterCommands<HouseCommandModule>();
            Bot.Instance.SlashCommands.RegisterCommands<IncarnationCommandModule>();
            Bot.Instance.SlashCommands.RegisterCommands<ItemCommandModule>();
            Bot.Instance.SlashCommands.RegisterCommands<ItemSetCommandModule>();
            Bot.Instance.SlashCommands.RegisterCommands<MapCommandModule>();
            Bot.Instance.SlashCommands.RegisterCommands<MonsterCommandModule>();
            Bot.Instance.SlashCommands.RegisterCommands<QuestCommandModule>();
            Bot.Instance.SlashCommands.RegisterCommands<RuneCommandModule>();
            Bot.Instance.SlashCommands.RegisterCommands<SpellCommandModule>();
        }
    }
}
