﻿using Cyberia.Api.Data;
using Cyberia.Api.Enums;
using Cyberia.Api.Utils;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands.Localization;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Cyberia.Salamandra.Commands.Dofus.Rune;

[Command(RuneInteractionLocalizer.CommandName), Description(RuneInteractionLocalizer.CommandDescription)]
[InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
[InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
[InteractionLocalizer<RuneInteractionLocalizer>]
public sealed class RuneCommandModule
{
    private readonly ICultureService _cultureService;
    private readonly DofusDatacenter _dofusDatacenter;
    private readonly IEmbedBuilderService _embedBuilderService;

    public RuneCommandModule(ICultureService cultureService, DofusDatacenter dofusDatacenter, IEmbedBuilderService embedBuilderService)
    {
        _cultureService = cultureService;
        _dofusDatacenter = dofusDatacenter;
        _embedBuilderService = embedBuilderService;
    }

    [Command(RuneInteractionLocalizer.Item_CommandName), Description(RuneInteractionLocalizer.Item_CommandDescription)]
    [InteractionLocalizer<RuneInteractionLocalizer>]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task ItemExecuteAsync(SlashCommandContext ctx,
        [Parameter(RuneInteractionLocalizer.Item_Value_ParameterName), Description(RuneInteractionLocalizer.Item_Value_ParameterDescription)]
        [InteractionLocalizer<RuneInteractionLocalizer>]
        [SlashAutoCompleteProvider<RuneItemAutocompleteProvider>]
        [MinMaxLength(1, 70)]
        string value,
        [Parameter(RuneInteractionLocalizer.Item_Quantity_ParameterName), Description(RuneInteractionLocalizer.Item_Quantity_ParameterDescription)]
        [InteractionLocalizer<RuneInteractionLocalizer>]
        [MinMaxValue(1, RuneItemMessageBuilder.MaxQuantity)]
        int quantity = 1)
    {
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        DiscordInteractionResponseBuilder? response = null;

        if (int.TryParse(value, out var itemId))
        {
            var itemData = _dofusDatacenter.ItemsRepository.GetItemDataById(itemId);
            if (itemData is not null)
            {
                response = await new RuneItemMessageBuilder(_embedBuilderService, itemData, quantity, culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }
        else
        {
            var itemsData = _dofusDatacenter.ItemsRepository.GetItemsDataByName(value, culture).ToList();
            if (itemsData.Count == 1)
            {
                response = await new RuneItemMessageBuilder(_embedBuilderService, itemsData[0], quantity, culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>();
            }
            else if (itemsData.Count > 1)
            {
                response = await new PaginatedRuneItemMessageBuilder(_embedBuilderService, itemsData, value, quantity, culture)
                    .BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }

        response ??= new DiscordInteractionResponseBuilder().WithContent(Translation.Get<BotTranslations>("Item.NotFound", culture));
        await ctx.RespondAsync(response);
    }

    [Command(RuneInteractionLocalizer.Stat_CommandName), Description(RuneInteractionLocalizer.Stat_CommandDescription)]
    [InteractionLocalizer<RuneInteractionLocalizer>]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task StatExecuteAsync(SlashCommandContext ctx,
        [Parameter(RuneInteractionLocalizer.Stat_ItemLvl_ParameterName), Description(RuneInteractionLocalizer.Stat_ItemLvl_ParameterDescription)]
        [InteractionLocalizer<RuneInteractionLocalizer>]
        [MinMaxValue(1, 200)]
        int itemLvl,
        [Parameter(RuneInteractionLocalizer.Stat_StatAmount_ParameterName), Description(RuneInteractionLocalizer.Stat_StatAmount_ParameterDescription)]
        [InteractionLocalizer<RuneInteractionLocalizer>]
        [MinMaxValue(1, 9999)]
        int statAmount,
        [Parameter(RuneInteractionLocalizer.Stat_runeName_ParameterName), Description(RuneInteractionLocalizer.Stat_runeName_ParameterDescription)]
        [InteractionLocalizer<RuneInteractionLocalizer>]
        [SlashAutoCompleteProvider<RuneAutocompleteProvider>]
        [MinMaxLength(1, 70)]
        int runeId)
    {
        var culture = await _cultureService.GetCultureAsync(ctx.Interaction);

        var runeData = _dofusDatacenter.RunesRepository.GetRuneDataById(runeId);
        if (runeData is null)
        {
            await ctx.RespondAsync(Translation.Get<BotTranslations>("Rune.NotFound", culture));
            return;
        }

        var percentRuneExtractable = Math.Round(RuneCalculator.GetPercentStatExtractable(runeData, itemLvl, statAmount), 2);

        var embed = _embedBuilderService.CreateBaseEmbedBuilder(EmbedCategory.Tools, Translation.Get<BotTranslations>("Embed.Rune.Author", culture), culture)
            .WithTitle(_dofusDatacenter.ItemsRepository.GetItemNameById(runeData.BaRuneItemId));

        if (statAmount == 1)
        {
            embed.WithDescription(Translation.Format(
                Translation.Get<BotTranslations>("Embed.Rune.Description.One", culture),
                Emojis.BaRune(runeData, culture),
                Formatter.Bold(percentRuneExtractable.ToString()),
                Formatter.Bold(itemLvl.ToString())));
        }
        else
        {
            embed.WithDescription(Translation.Format(
                Translation.Get<BotTranslations>("Embed.Rune.Description.Multiple", culture),
                Emojis.BaRune(runeData, culture),
                Formatter.Bold(percentRuneExtractable.ToString()),
                Formatter.Bold(itemLvl.ToString()),
                Formatter.Bold(statAmount.ToString())));

            if (runeData.PaRuneItemId is null && runeData.RaRuneItemId is null)
            {
                var runeBundle = RuneCalculator.GetRuneBundleFromStat(runeData, itemLvl, statAmount, RuneCalculator.AverageMultiplicator);

                embed.AddField(Translation.Get<BotTranslations>("ShortAverage", culture), $"{Emojis.BaRune(runeData, culture)} {Formatter.Bold(runeBundle.BaQuantity.ToString())}");
            }
            else
            {
                var minRuneBundle = RuneCalculator.GetRuneBundleFromStat(runeData, itemLvl, statAmount, RuneCalculator.MinMultiplicator);
                var averageRuneBundle = RuneCalculator.GetRuneBundleFromStat(runeData, itemLvl, statAmount, RuneCalculator.AverageMultiplicator);
                var maxRuneBundle = RuneCalculator.GetRuneBundleFromStat(runeData, itemLvl, statAmount, RuneCalculator.MaxMultiplicator);

                embed.AddField(Translation.Get<BotTranslations>("ShortMinimum", culture), GetRuneBundleContentFieldStatCommand(minRuneBundle, culture), true);
                embed.AddField(Translation.Get<BotTranslations>("ShortAverage", culture), GetRuneBundleContentFieldStatCommand(averageRuneBundle, culture), true);
                embed.AddField(Translation.Get<BotTranslations>("ShortMaximum", culture), GetRuneBundleContentFieldStatCommand(maxRuneBundle, culture), true);

                if (runeData.RaRuneItemId is not null && maxRuneBundle.RaQuantity == 1 && minRuneBundle.RaQuantity == 0)
                {
                    var percentRaObtention = Math.Round(RuneCalculator.GetPercentChanceToObtainRune(runeData, RuneType.RA, itemLvl, statAmount), 2);
                    embed.AddField(Translation.Get<BotTranslations>("Embed.Field.RuneRaRate.Title", culture), $"{Emojis.RaRune(runeData, culture)} {Formatter.Bold(percentRaObtention.ToString())}%");
                }
                else if (runeData.PaRuneItemId is not null && maxRuneBundle.PaQuantity == 1 && minRuneBundle.PaQuantity == 0)
                {
                    var percentPaObtention = Math.Round(RuneCalculator.GetPercentChanceToObtainRune(runeData, RuneType.PA, itemLvl, statAmount), 2);
                    embed.AddField(Translation.Get<BotTranslations>("Embed.Field.RunePaRate.Title", culture), $"{Emojis.PaRune(runeData, culture)} {Formatter.Bold(percentPaObtention.ToString())}%");
                }
            }
        }

        embed.AddField(Translation.Get<BotTranslations>("Embed.Field.Source.Title", culture), RuneCalculator.Source);

        await ctx.RespondAsync(embed);
    }

    private static string GetRuneBundleContentFieldStatCommand(RuneBundle runeBundle, CultureInfo? culture)
    {
        StringBuilder builder = new();

        builder.Append(Emojis.BaRune(runeBundle.RuneData, culture));
        builder.Append(' ');
        builder.Append(Formatter.Bold(runeBundle.BaQuantity.ToFormattedString(culture)));

        if (runeBundle.RemainingBaPercent > 0)
        {
            builder.Append('+');
            builder.Append(Math.Round(runeBundle.RemainingBaPercent, 2));
            builder.Append('%');
        }

        if (runeBundle.PaQuantity > 0)
        {
            builder.Append('\n');
            builder.Append(Emojis.PaRune(runeBundle.RuneData, culture));
            builder.Append(' ');
            builder.Append(Formatter.Bold(runeBundle.PaQuantity.ToFormattedString(culture)));
        }

        if (runeBundle.RaQuantity > 0)
        {
            builder.Append('\n');
            builder.Append(Emojis.RaRune(runeBundle.RuneData, culture));
            builder.Append(' ');
            builder.Append(Formatter.Bold(runeBundle.RaQuantity.ToFormattedString(culture)));
        }

        return builder.ToString();
    }
}
