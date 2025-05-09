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

namespace Cyberia.Salamandra.Commands.Dofus.Crit;

public sealed class CritCommandModule
{
    private readonly ICultureService _cultureService;
    private readonly IEmbedBuilderService _embedBuilderService;

    public CritCommandModule(ICultureService cultureService, IEmbedBuilderService embedBuilderService)
    {
        _cultureService = cultureService;
        _embedBuilderService = embedBuilderService;
    }

    [Command(CritInteractionLocalizer.CommandName), Description(CritInteractionLocalizer.CommandDescription)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    [InteractionLocalizer<CritInteractionLocalizer>]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter(CritInteractionLocalizer.BaseRate_ParameterName), Description(CritInteractionLocalizer.BaseRate_ParameterDescription)]
        [InteractionLocalizer<CritInteractionLocalizer>]
        [MinMaxValue(1, 999)]
        int baseRate,
        [Parameter(CritInteractionLocalizer.CriticalHitBonus_ParameterName), Description(CritInteractionLocalizer.CriticalHitBonus_ParameterDescription)]
        [InteractionLocalizer<CritInteractionLocalizer>]
        [MinMaxValue(1, 999)]
        int criticalHitBonus,
        [Parameter(CritInteractionLocalizer.Agility_ParameterName) , Description(CritInteractionLocalizer.Agility_ParameterDescription)]
        [InteractionLocalizer<CritInteractionLocalizer>]
        [MinMaxValue(1, 99999)]
        int agility)
    {
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        var rate = Formulas.GetCriticalHitRate(baseRate, criticalHitBonus, agility);
        var agilityNeeded = Formulas.GetAgilityForOptimalCriticalHitRate(baseRate, criticalHitBonus);

        var embed = _embedBuilderService.CreateEmbedBuilder(EmbedCategory.Tools, Translation.Get<BotTranslations>("Embed.Crit.Author", culture), culture)
            .WithDescription(Translation.Format(
                Translation.Get<BotTranslations>("Embed.Crit.Description", culture),
                Formatter.Bold(rate.ToString()),
                Formatter.Bold(baseRate.ToString()),
                Formatter.Bold(criticalHitBonus.ToString()),
                Formatter.Bold(agility.ToFormattedString(culture)),
                Formatter.Bold(agilityNeeded.ToFormattedString(culture))));

        await ctx.RespondAsync(embed);
    }
}
