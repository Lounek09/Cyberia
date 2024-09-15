using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.EventHandlers;
using Cyberia.Salamandra.Services;

using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.Localization;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;
using System.Text;

namespace Cyberia.Salamandra.Commands.Other.Help;

public sealed class HelpCommandModule
{
    private readonly CultureService _cultureService;
    private readonly EmbedBuilderService _embedBuilderService;

    public HelpCommandModule(CultureService cultureService, EmbedBuilderService embedBuilderService)
    {
        _cultureService = cultureService;
        _embedBuilderService = embedBuilderService;
    }

    [Command(HelpInteractionLocalizer.CommandName), Description(HelpInteractionLocalizer.CommandDescription)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    [InteractionLocalizer<HelpInteractionLocalizer>]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task ExecuteAsync(SlashCommandContext ctx)
    {
        await _cultureService.SetCultureAsync(ctx.Interaction);
        var locale = ctx.Interaction.Locale ?? ctx.Interaction.GuildLocale ?? string.Empty;

        StringBuilder descriptionBuilder = new();

        var commands = await ctx.Client.GetGlobalApplicationCommandsAsync(true);

        foreach (var command in commands)
        {
            if (command.Name.Equals(HelpInteractionLocalizer.CommandName))
            {
                continue;
            }

            if (command.DescriptionLocalizations is null || !command.DescriptionLocalizations.TryGetValue(locale, out var commandDescription))
            {
                commandDescription = command.Description;
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
                    descriptionBuilder.Append(commandDescription);
                    descriptionBuilder.Append('\n');

                    continue;
                }
            }

            descriptionBuilder.Append("- ");
            descriptionBuilder.Append(command.Mention);
            descriptionBuilder.Append(" : ");
            descriptionBuilder.Append(commandDescription);
            descriptionBuilder.Append('\n');
        }

        await ctx.RespondAsync(_embedBuilderService.CreateEmbedBuilder(EmbedCategory.Tools, "Help")
            .WithDescription(descriptionBuilder.ToString()));
    }
}
