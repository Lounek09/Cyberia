using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.SlashCommands;

using System.Text;
namespace Cyberia.Salamandra.Commands.Other;

public sealed class HelpCommandModule : ApplicationCommandModule
{
    [SlashCommand("help", "Liste les commandes du bot")]
    public async Task Command(InteractionContext ctx)
    {
        var commands = Bot.SlashCommands.RegisteredCommands
            .Where(x => x.Key is null)
            .FirstOrDefault().Value;

        StringBuilder descriptionBuilder = new();

        foreach (var command in commands)
        {
            if (command.Name.Equals("help"))
            {
                continue;
            }

            if (command.Options is not null)
            {
                var subCommands = command.Options
                    .Where(x => x.Type is ApplicationCommandOptionType.SubCommand);

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

        var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Tools, "Help")
            .WithDescription(descriptionBuilder.ToString());

        await ctx.CreateResponseAsync(embed);
    }
}
