using DSharpPlus.Commands;

namespace Cyberia.Salamandra.Commands.Dofus;

public static class DofusCommandsGroup
{
    public static void RegisterDofusCommands(this CommandsExtension extension)
    {
        extension.AddCommand(BreedCommandModule.ExecuteAsync);
        extension.AddCommand(CraftCommandModule.ExecuteAsync);
        extension.AddCommand(CritCommandModule.ExecuteAsync);
        extension.AddCommand(EscapeCommandModule.ExecuteAsync);
        extension.AddCommands<HouseCommandModule>();
        extension.AddCommand(IncarnationCommandModule.ExecuteAsync);
        extension.AddCommand(ItemCommandModule.ExecuteAsync);
        extension.AddCommand(ItemSetCommandModule.ExecuteAsync);
        extension.AddCommands<MapCommandModule>();
        extension.AddCommand(MonsterCommandModule.ExecuteAsync);
        extension.AddCommand(QuestCommandModule.ExecuteAsync);
        extension.AddCommands<RuneCommandModule>();
        extension.AddCommand(SpellCommandModule.ExecuteAsync);
    }
}
