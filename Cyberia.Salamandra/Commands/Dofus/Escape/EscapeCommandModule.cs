using Cyberia.Api;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.EventHandlers;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.Localization;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Dofus.Escape;

public sealed class EscapeCommandModule
{
    private readonly CultureService _cultureService;
    private readonly EmbedBuilderService _embedBuilderService;

    public EscapeCommandModule(CultureService cultureService, EmbedBuilderService embedBuilderService)
    {
        _cultureService = cultureService;
        _embedBuilderService = embedBuilderService;
    }

    [Command(EscapeInteractionLocalizer.CommandName), Description(EscapeInteractionLocalizer.CommandDescription)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    [InteractionLocalizer<EscapeInteractionLocalizer>]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter(EscapeInteractionLocalizer.Agility_ParameterName), Description(EscapeInteractionLocalizer.Agility_ParameterDescription)]
        [InteractionLocalizer<EscapeInteractionLocalizer>]
        [MinMaxValue(1, 99999)]
        int agility,
        [Parameter(EscapeInteractionLocalizer.EnemyAgility_ParameterName), Description(EscapeInteractionLocalizer.EnemyAgility_ParameterDescription)]
        [InteractionLocalizer<EscapeInteractionLocalizer>]
        [MinMaxValue(1, 99999)]
        int enemyAgility)
    {
        using CultureScope scope = new(await _cultureService.GetCultureAsync(ctx.Interaction));

        var escapePercent = Formulas.GetEscapePercent(agility, enemyAgility);
        var agilityToEscapeForSure = Formulas.GetAgilityToEscapeForSure(enemyAgility);

        var embed = _embedBuilderService.CreateEmbedBuilder(EmbedCategory.Tools, BotTranslations.Embed_Escape_Author)
            .WithDescription(Translation.Format(
                BotTranslations.Embed_Escape_Description,
                Formatter.Bold(agility.ToString()),
                Formatter.Bold(escapePercent.ToString()),
                Formatter.Bold(enemyAgility.ToString()),
                Formatter.Bold(agilityToEscapeForSure.ToString())));

        await ctx.RespondAsync(embed);
    }
}
