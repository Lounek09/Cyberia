using DSharpPlus.Commands;

using System.Reflection;

namespace Cyberia.Salamandra.Extensions.DSharpPlus;

/// <summary>
/// Provides extension methods for the <see cref="CommandsExtension"/> class.
/// </summary>
public static class CommandsExtensionExtensions
{
    /// <summary>
    /// Registers the commands from the assembly.
    /// </summary>
    /// <param name="extension">The extension to register the commands to.</param>
    /// <param name="guildIds">The IDs of the guilds to register the admin commands to.</param>
    public static void RegisterCommands(this CommandsExtension extension, params ulong[] guildIds)
    {
        Dictionary<string, bool> commandGroups = new()
        {
            { "Cyberia.Salamandra.Commands.Admin", true },
            { "Cyberia.Salamandra.Commands.Data", true },
            { "Cyberia.Salamandra.Commands.Dofus", false },
            { "Cyberia.Salamandra.Commands.Other", false }
        };

        var types = Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => !string.IsNullOrEmpty(x.Namespace) && x.Name.EndsWith("CommandModule"));

        foreach (var (startNamespace, requiresGuildId) in commandGroups)
        {
            var commandModules = types.Where(x => x.Namespace!.StartsWith(startNamespace));

            if (requiresGuildId)
            {
                extension.AddCommands(commandModules, guildIds);
            }
            else
            {
                extension.AddCommands(commandModules);
            }
        }
    }
}
