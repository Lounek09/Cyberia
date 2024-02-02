using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus;

public static class DofusCommandsGroup
{
    public static void RegisterDofusCommands(this SlashCommandsExtension extension)
    {
        extension.RegisterCommands<BreedCommandModule>();
        extension.RegisterCommands<CraftCommandModule>();
        extension.RegisterCommands<CritCommandModule>();
        extension.RegisterCommands<EscapeCommandModule>();
        extension.RegisterCommands<HouseCommandModule>();
        extension.RegisterCommands<IncarnationCommandModule>();
        extension.RegisterCommands<ItemCommandModule>();
        extension.RegisterCommands<ItemSetCommandModule>();
        extension.RegisterCommands<MapCommandModule>();
        extension.RegisterCommands<MonsterCommandModule>();
        extension.RegisterCommands<QuestCommandModule>();
        extension.RegisterCommands<RuneCommandModule>();
        extension.RegisterCommands<SpellCommandModule>();
    }
}
