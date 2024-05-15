using Cyberia.Api;
using Cyberia.Api.Managers;
using Cyberia.Api.Values;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;
using System.Text;

namespace Cyberia.Salamandra.Commands.Dofus.Rune;

[Command("rune"), Description("Permet de calculer les runes obtenues lors d'un brisage")]
[InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
[InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
public sealed class RuneCommandModule
{
    [Command("item"), Description("Permet de calculer le nombre de rune obtenable depuis un item")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public static async Task ItemExecuteAsync(SlashCommandContext ctx,
        [Parameter("item"), Description("Nom de l'item à briser")]
        [SlashAutoCompleteProvider<RuneItemAutocompleteProvider>]
        [MinMaxLength(1, 70)]
        string value,
        [Parameter("quantite"), Description("Quantité d'item")]
        [MinMaxValue(1, RuneItemMessageBuilder.MaxQte)]
        int qte = 1)
    {
        DiscordInteractionResponseBuilder? response = null;

        if (int.TryParse(value, out var itemId))
        {
            var itemData = DofusApi.Datacenter.ItemsRepository.GetItemDataById(itemId);
            if (itemData is not null)
            {
                response = await new RuneItemMessageBuilder(itemData, qte).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
        }
        else
        {
            var itemsData = DofusApi.Datacenter.ItemsRepository.GetItemsDataByName(value).ToList();
            if (itemsData.Count == 1)
            {
                response = await new RuneItemMessageBuilder(itemsData[0], qte).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
            else if (itemsData.Count > 1)
            {
                response = await new PaginatedRuneItemMessageBuilder(itemsData, value, qte).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
        }

        response ??= new DiscordInteractionResponseBuilder().WithContent("Item introuvable");
        await ctx.RespondAsync(response);
    }

    [Command("stat"), Description("Permet de calculer le nombre de rune obtenable d'une stat sur un item")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public static async Task StatExecuteAsync(SlashCommandContext ctx,
        [Parameter("niveau"), Description("Niveau de l'item")]
        [MinMaxValue(1, 200)]
        int itemLvl,
        [Parameter("montant") , Description("Montant de la stat")]
        [MinMaxValue(1, 9999)]
        int statAmount,
        [Parameter("rune"), Description("Nom de la rune")]
        [SlashAutoCompleteProvider<RuneAutocompleteProvider>]
        [MinMaxLength(1, 70)]
        string runeName)
    {
        var runeData = DofusApi.Datacenter.RunesRepository.GetRuneDataByName(runeName);
        if (runeData is null)
        {
            await ctx.RespondAsync($"Paramètre incorrect\n{Formatter.Italic("Valeur possible :")} {Formatter.InlineCode(DofusApi.Datacenter.RunesRepository.GetAllRuneName())}");
            return;
        }

        var percentRuneExtractable = Math.Round(RuneManager.GetPercentStatExtractable(runeData, itemLvl, statAmount), 2);
        if (percentRuneExtractable == -1)
        {
            await ctx.RespondAsync("Une erreur est survenue lors du calcul du % de statistique extractible");
            return;
        }

        var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Tools, "Calculateur de runes")
            .WithTitle(runeData.GetFullName());

        if (statAmount == 1)
        {
            embed.WithDescription($"{Emojis.BaRune(runeData.Id)} {Formatter.Bold(percentRuneExtractable.ToString())}% de chance sur un objet de niveau {Formatter.Bold(itemLvl.ToString())}");
        }
        else
        {
            embed.WithDescription($"{Formatter.Bold(percentRuneExtractable.ToString())}% de puissance extractible sur un objet niveau {Formatter.Bold(itemLvl.ToString())} comprenant {Formatter.Bold(statAmount.ToString())} stats");

            if (!runeData.HasPa && !runeData.HasRa)
            {
                var runeBundle = RuneManager.GetRuneBundleFromStat(runeData, itemLvl, statAmount, RuneManager.AverageMultiplicator);

                embed.AddField("Moy.", $"{Emojis.BaRune(runeData.Id)} {Formatter.Bold(runeBundle.BaAmount.ToString())}");
            }
            else
            {
                var minRuneBundle = RuneManager.GetRuneBundleFromStat(runeData, itemLvl, statAmount, RuneManager.MinMultiplicator);
                var averageRuneBundle = RuneManager.GetRuneBundleFromStat(runeData, itemLvl, statAmount, RuneManager.AverageMultiplicator);
                var maxRuneBundle = RuneManager.GetRuneBundleFromStat(runeData, itemLvl, statAmount, RuneManager.MaxMultiplicator);

                embed.AddField("Min.", GetRuneBundleTextFieldStatCommand(minRuneBundle), true);
                embed.AddField("Moy.", GetRuneBundleTextFieldStatCommand(averageRuneBundle), true);
                embed.AddField("Max.", GetRuneBundleTextFieldStatCommand(maxRuneBundle), true);

                if (runeData.HasRa && maxRuneBundle.RaAmount == 1 && minRuneBundle.RaAmount == 0)
                {
                    var percentRaObtention = Math.Round(RuneManager.GetPercentToObtainRune(runeData, RuneType.RA, itemLvl, statAmount), 2);
                    embed.AddField("Taux Ra :", $"{Emojis.RaRune(runeData.Id)} {Formatter.Bold(percentRaObtention.ToString())}%");
                }
                else if (runeData.HasPa && maxRuneBundle.PaAmount == 1 && minRuneBundle.PaAmount == 0)
                {
                    var percentPaObtention = Math.Round(RuneManager.GetPercentToObtainRune(runeData, RuneType.PA, itemLvl, statAmount), 2);
                    embed.AddField("Taux Pa :", $"{Emojis.PaRune(runeData.Id)} {Formatter.Bold(percentPaObtention.ToString())}%");
                }
            }
        }

        embed.AddField("Source :", "https://forums.jeuxonline.info/sujet/1045383/les-taux-de-brisage");

        await ctx.RespondAsync(embed);
    }

    private static string GetRuneBundleTextFieldStatCommand(RuneBundle runeBundle)
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
