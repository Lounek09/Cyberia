using DSharpPlus.Commands;

using System.Reflection;

namespace Cyberia.Salamandra.Extensions.DSharpPlus;

/// <summary>
/// Provides extension methods for the <see cref="CommandsExtension"/> class.
/// </summary>
public static class CommandsExtensionExtensions
{
    extension(CommandsExtension extension)
    {
        /// <summary>
        /// Registers the commands from the assembly.
        /// </summary>
        /// <param name="adminGuildIds">The guild IDs to register the admin commands to.</param>
        public void RegisterCommands(params ulong[] adminGuildIds)
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

            foreach (var (startNamespace, isAdminCommands) in commandGroups)
            {
                var commandModules = types.Where(x => x.Namespace!.StartsWith(startNamespace));

                if (isAdminCommands)
                {
                    extension.AddCommands(commandModules, adminGuildIds);
                }
                else
                {
                    extension.AddCommands(commandModules);
                }
            }
        }
    }
}
