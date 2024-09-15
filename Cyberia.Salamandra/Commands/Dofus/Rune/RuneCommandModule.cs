using Cyberia.Api;
using Cyberia.Api.Managers;
using Cyberia.Api.Values;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.EventHandlers;
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
using System.Text;

namespace Cyberia.Salamandra.Commands.Dofus.Rune;

[Command(RuneInteractionLocalizer.CommandName), Description(RuneInteractionLocalizer.CommandDescription)]
[InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
[InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
[InteractionLocalizer<RuneInteractionLocalizer>]
public sealed class RuneCommandModule
{
    private readonly CultureService _cultureService;
    private readonly EmbedBuilderService _embedBuilderService;

    public RuneCommandModule(CultureService cultureService, EmbedBuilderService embedBuilderService)
    {
        _cultureService = cultureService;
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
        await _cultureService.SetCultureAsync(ctx.Interaction);

        DiscordInteractionResponseBuilder? response = null;

        if (int.TryParse(value, out var itemId))
        {
            var itemData = DofusApi.Datacenter.ItemsRepository.GetItemDataById(itemId);
            if (itemData is not null)
            {
                response = await new RuneItemMessageBuilder(_embedBuilderService, itemData, quantity).BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }
        else
        {
            var itemsData = DofusApi.Datacenter.ItemsRepository.GetItemsDataByName(value).ToList();
            if (itemsData.Count == 1)
            {
                response = await new RuneItemMessageBuilder(_embedBuilderService, itemsData[0], quantity).BuildAsync<DiscordInteractionResponseBuilder>();
            }
            else if (itemsData.Count > 1)
            {
                response = await new PaginatedRuneItemMessageBuilder(_embedBuilderService, itemsData, value, quantity).BuildAsync<DiscordInteractionResponseBuilder>();
            }
        }

        response ??= new DiscordInteractionResponseBuilder().WithContent(BotTranslations.Item_NotFound);
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
        string runeName)
    {
        await _cultureService.SetCultureAsync(ctx.Interaction);

        var runeData = DofusApi.Datacenter.RunesRepository.GetRuneDataByName(runeName);
        if (runeData is null)
        {
            await ctx.RespondAsync($"""
                {BotTranslations.IncorrectParameter}
                {Formatter.Italic(BotTranslations.PossibleValues)} {Formatter.InlineCode(DofusApi.Datacenter.RunesRepository.GetAllRuneName())}
                """);
            return;
        }

        var percentRuneExtractable = Math.Round(RuneManager.GetPercentStatExtractable(runeData, itemLvl, statAmount), 2);

        var embed = _embedBuilderService.CreateEmbedBuilder(EmbedCategory.Tools, BotTranslations.Embed_Rune_Author)
            .WithTitle(runeData.GetFullName());

        if (statAmount == 1)
        {
            embed.WithDescription(Translation.Format(
                BotTranslations.Embed_Rune_Description_One,
                Emojis.BaRune(runeData.Id),
                Formatter.Bold(percentRuneExtractable.ToString()),
                Formatter.Bold(itemLvl.ToString())));
        }
        else
        {
            embed.WithDescription(Translation.Format(
                BotTranslations.Embed_Rune_Description_Multiple,
                Emojis.BaRune(runeData.Id),
                Formatter.Bold(percentRuneExtractable.ToString()),
                Formatter.Bold(itemLvl.ToString()),
                Formatter.Bold(statAmount.ToString())));

            if (!runeData.HasPa && !runeData.HasRa)
            {
                var runeBundle = RuneManager.GetRuneBundleFromStat(runeData, itemLvl, statAmount, RuneManager.AverageMultiplicator);

                embed.AddField(BotTranslations.ShortAverage, $"{Emojis.BaRune(runeData.Id)} {Formatter.Bold(runeBundle.BaAmount.ToString())}");
            }
            else
            {
                var minRuneBundle = RuneManager.GetRuneBundleFromStat(runeData, itemLvl, statAmount, RuneManager.MinMultiplicator);
                var averageRuneBundle = RuneManager.GetRuneBundleFromStat(runeData, itemLvl, statAmount, RuneManager.AverageMultiplicator);
                var maxRuneBundle = RuneManager.GetRuneBundleFromStat(runeData, itemLvl, statAmount, RuneManager.MaxMultiplicator);

                embed.AddField(BotTranslations.ShortMinimum, GetRuneBundleContentFieldStatCommand(minRuneBundle), true);
                embed.AddField(BotTranslations.ShortAverage, GetRuneBundleContentFieldStatCommand(averageRuneBundle), true);
                embed.AddField(BotTranslations.ShortMaximum, GetRuneBundleContentFieldStatCommand(maxRuneBundle), true);

                if (runeData.HasRa && maxRuneBundle.RaAmount == 1 && minRuneBundle.RaAmount == 0)
                {
                    var percentRaObtention = Math.Round(RuneManager.GetPercentToObtainRune(runeData, RuneType.RA, itemLvl, statAmount), 2);
                    embed.AddField(BotTranslations.Embed_Field_RuneRaRate_Title, $"{Emojis.RaRune(runeData.Id)} {Formatter.Bold(percentRaObtention.ToString())}%");
                }
                else if (runeData.HasPa && maxRuneBundle.PaAmount == 1 && minRuneBundle.PaAmount == 0)
                {
                    var percentPaObtention = Math.Round(RuneManager.GetPercentToObtainRune(runeData, RuneType.PA, itemLvl, statAmount), 2);
                    embed.AddField(BotTranslations.Embed_Field_RunePaRate_Title, $"{Emojis.PaRune(runeData.Id)} {Formatter.Bold(percentPaObtention.ToString())}%");
                }
            }
        }

        embed.AddField(BotTranslations.Embed_Field_Source_Title, "https://forums.jeuxonline.info/sujet/1045383/les-taux-de-brisage");

        await ctx.RespondAsync(embed);
    }

    private static string GetRuneBundleContentFieldStatCommand(RuneBundle runeBundle)
    {
        StringBuilder builder = new();

        builder.Append(Emojis.BaRune(runeBundle.RuneData.Id));
        builder.Append(' ');
        builder.Append(Formatter.Bold(runeBundle.BaAmount.ToFormattedString()));

        if (runeBundle.RemainingBaPercent > 0)
        {
            builder.Append('+');
            builder.Append(Math.Round(runeBundle.RemainingBaPercent, 2));
            builder.Append('%');
        }

        if (runeBundle.PaAmount > 0)
        {
            builder.Append('\n');
            builder.Append(Emojis.PaRune(runeBundle.RuneData.Id));
            builder.Append(' ');
            builder.Append(Formatter.Bold(runeBundle.PaAmount.ToFormattedString()));
        }

        if (runeBundle.RaAmount > 0)
        {
            builder.Append('\n');
            builder.Append(Emojis.RaRune(runeBundle.RuneData.Id));
            builder.Append(' ');
            builder.Append(Formatter.Bold(runeBundle.RaAmount.ToFormattedString()));
        }

        return builder.ToString();
    }
}
