using Cyberia.Api.Utils;
using Cyberia.Salamandra.Enums;
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
    private readonly ICultureService _cultureService;
    private readonly IEmbedBuilderService _embedBuilderService;

    public EscapeCommandModule(ICultureService cultureService, IEmbedBuilderService embedBuilderService)
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
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        var escapePercentProbability = Formulas.GetEscapePercentProbability(agility, enemyAgility);
        var agilityForGuarenteedEscape = Formulas.GetAgilityForGuaranteedEscape(enemyAgility);

        var embed = _embedBuilderService.CreateBaseEmbedBuilder(EmbedCategory.Tools, Translation.Get<BotTranslations>("Embed.Escape.Author", culture), culture)
            .WithDescription(Translation.Format(
                Translation.Get<BotTranslations>("Embed.Escape.Description", culture),
                Formatter.Bold(agility.ToFormattedString(culture)),
                Formatter.Bold(escapePercentProbability.ToString()),
                Formatter.Bold(enemyAgility.ToFormattedString(culture)),
                Formatter.Bold(agilityForGuarenteedEscape.ToFormattedString(culture))));

        await ctx.RespondAsync(embed);
    }
}
