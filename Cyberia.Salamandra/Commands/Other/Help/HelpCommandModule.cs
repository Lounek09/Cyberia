using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;
using System.Text;

namespace Cyberia.Salamandra.Commands.Other;

public sealed class HelpCommandModule
{
    [Command("help"), Description("Liste les commandes du bot")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    public static async Task ExecuteAsync(SlashCommandContext ctx)
    {
        StringBuilder descriptionBuilder = new();

        var commands = await Bot.Client.GetGlobalApplicationCommandsAsync();

        foreach (var command in commands)
        {
            if (command.Name.Equals("help"))
            {
                continue;
            }

            if (command.Options is not null)
            {
                var subCommands = command.Options
                    .Where(x => x.Type is DiscordApplicationCommandOptionType.SubCommand);

                if (subCommands.Any())
                {
                    descriptionBuilder.Append("- ");
                    descriptionBuilder.Append(string.Join(" - ", subCommands.Select(x => command.GetSubcommandMention(x.Name))));
                    descriptionBuilder.Append(" : ");
                    descriptionBuilder.Append(command.Description);
                    descriptionBuilder.Append('\n');

                    continue;
                }
            }

            descriptionBuilder.Append("- ");
            descriptionBuilder.Append(command.Mention);
            descriptionBuilder.Append(" : ");
            descriptionBuilder.Append(command.Description);
            descriptionBuilder.Append('\n');
        }

        await ctx.RespondAsync(EmbedManager.CreateEmbedBuilder(EmbedCategory.Tools, "Help")
            .WithDescription(descriptionBuilder.ToString()));
    }
}
