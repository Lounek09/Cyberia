using Cyberia.Api;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;
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
    private readonly CultureService _cultureService;

    public CritCommandModule(CultureService cultureService)
    {
        _cultureService = cultureService;
    }

    [Command(CritInteractionLocalizer.CommandName), Description(CritInteractionLocalizer.CommandDescription)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    [InteractionLocalizer<CritInteractionLocalizer>]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter(CritInteractionLocalizer.Number_ParameterName), Description(CritInteractionLocalizer.Number_ParameterDescription)]
        [InteractionLocalizer<CritInteractionLocalizer>]
        [MinMaxValue(1, 999)]
        int number,
        [Parameter(CritInteractionLocalizer.TargetRate_ParameterName), Description(CritInteractionLocalizer.TargetRate_ParameterDescription)]
        [InteractionLocalizer<CritInteractionLocalizer>]
        [MinMaxValue(1, 999)]
        int targetRate,
        [Parameter(CritInteractionLocalizer.Agility_ParameterName) , Description(CritInteractionLocalizer.Agility_ParameterDescription)]
        [InteractionLocalizer<CritInteractionLocalizer>]
        [MinMaxValue(1, 99999)]
        int agility)
    {
        await _cultureService.SetCultureAsync(ctx.Interaction);

        var rate = Formulas.GetCriticalRate(number, targetRate, agility);
        var agilityNeeded = Formulas.GetAgilityForHalfCriticalRate(number, targetRate);

        var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Tools, BotTranslations.Embed_Crit_Author)
            .WithDescription(Translation.Format(
                BotTranslations.Embed_Crit_Description,
                Formatter.Bold($"1/{rate}"),
                Formatter.Bold($"1/{targetRate}"),
                Formatter.Bold(number.ToString()),
                Formatter.Bold(agility.ToString()),
                Formatter.Bold(agilityNeeded.ToString())));

        await ctx.RespondAsync(embed);
    }
}
