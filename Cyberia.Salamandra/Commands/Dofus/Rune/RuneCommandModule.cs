using Cyberia.Api;
using Cyberia.Api.Managers;
using Cyberia.Api.Values;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using System.Text;

namespace Cyberia.Salamandra.Commands.Dofus;

[SlashCommandGroup("rune", "Permet de calculer les runes obtenues lors d'un brisage")]
public sealed class RuneCommandModule : ApplicationCommandModule
{
    [SlashCommand("item", "Permet de calculer le nombre de rune obtenable depuis un item")]
    public async Task ItemCommand(InteractionContext ctx,
        [Option("quantite", "Quantité d'item")]
        [Minimum(1), Maximum(RuneItemMessageBuilder.MAX_QTE)]
        long qte,
        [Option("item", "Nom de l'item à briser", true)]
        [Autocomplete(typeof(RuneItemAutocompleteProvider))]
        string value)
    {
        DiscordInteractionResponseBuilder? response = null;

        if (int.TryParse(value, out var itemId))
        {
            var itemData = DofusApi.Datacenter.ItemsData.GetItemDataById(itemId);
            if (itemData is not null)
            {
                response = await new RuneItemMessageBuilder(itemData, (int)qte).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
        }
        else
        {
            var itemsData = DofusApi.Datacenter.ItemsData.GetItemsDataByName(value).ToList();
            if (itemsData.Count == 1)
            {
                response = await new RuneItemMessageBuilder(itemsData[0], (int)qte).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
            else if (itemsData.Count > 1)
            {
                response = await new PaginatedRuneItemMessageBuilder(itemsData, value, (int)qte).GetMessageAsync<DiscordInteractionResponseBuilder>();
            }
        }

        response ??= new DiscordInteractionResponseBuilder().WithContent("Item introuvable");
        await ctx.CreateResponseAsync(response);
    }

    [SlashCommand("stat", "Permet de calculer le nombre de rune obtenable d'une stat sur un item")]
    public async Task StatCommand(InteractionContext ctx,
        [Option("niveau", "Niveau de l'item")]
        [Minimum(1), Maximum(200)]
        long itemLvllong,
        [Option("montant", "Montant de la stat")]
        [Minimum(1), Maximum(9999)]
        long statAmountlong,
        [Option("rune", "Nom de la rune", true)]
        [Autocomplete(typeof(RuneAutocompleteProvider))]
        string name)
    {
        var runeData = DofusApi.Datacenter.RunesData.GetRuneDataByName(name);
        if (runeData is null)
        {
            await ctx.CreateResponseAsync($"Paramètre incorrect\n{Formatter.Italic("Valeur possible :")} {Formatter.InlineCode(DofusApi.Datacenter.RunesData.GetAllRuneName())}");
            return;
        }

        var itemLvl = (int)itemLvllong;
        var statAmount = (int)statAmountlong;

        var percentRuneExtractable = Math.Round(RuneManager.GetPercentStatExtractable(runeData, itemLvl, statAmount), 2);
        if (percentRuneExtractable == -1)
        {
            await ctx.CreateResponseAsync("Une erreur est survenue lors du calcul du % de statistique extractible");
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
                var runeBundle = RuneManager.GetRuneBundleFromStat(runeData, itemLvl, statAmount, RuneManager.AVERAGE_MULTIPLICATOR);

                embed.AddField("Moy.", $"{Emojis.BaRune(runeData.Id)} {Formatter.Bold(runeBundle.BaAmount.ToString())}");
            }
            else
            {
                var minRuneBundle = RuneManager.GetRuneBundleFromStat(runeData, itemLvl, statAmount, RuneManager.MIN_MULTIPLICATOR);
                var averageRuneBundle = RuneManager.GetRuneBundleFromStat(runeData, itemLvl, statAmount, RuneManager.AVERAGE_MULTIPLICATOR);
                var maxRuneBundle = RuneManager.GetRuneBundleFromStat(runeData, itemLvl, statAmount, RuneManager.MAX_MULTIPLICATOR);

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

        await ctx.CreateResponseAsync(embed);
    }

    private static string GetRuneBundleTextFieldStatCommand(RuneBundle runeBundle)
    {
        StringBuilder builder = new();

        builder.Append(Emojis.BaRune(runeBundle.RuneData.Id));
        builder.Append(' ');
        builder.Append(Formatter.Bold(runeBundle.BaAmount.ToStringThousandSeparator()));

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
            builder.Append(Formatter.Bold(runeBundle.PaAmount.ToStringThousandSeparator()));
        }

        if (runeBundle.RaAmount > 0)
        {
            builder.Append('\n');
            builder.Append(Emojis.RaRune(runeBundle.RuneData.Id));
            builder.Append(' ');
            builder.Append(Formatter.Bold(runeBundle.RaAmount.ToStringThousandSeparator()));
        }

        return builder.ToString();
    }
}
