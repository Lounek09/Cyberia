using Cyberia.Api.Data;
using Cyberia.Api.Managers;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
    public sealed class RuneCommandModule : ApplicationCommandModule
    {
        [SlashCommand("rune", "Permet de calculer le nombre de rune obtenable d'une stat sur un item")]
        public async Task Command(InteractionContext ctx,
            [Option("niveau", "Niveau de l'item")]
            [Minimum(1), Maximum(200)]
            long itemLevel,
            [Option("montant", "Montant de la stat")]
            [Minimum(1), Maximum(9999)]
            long statAmount,
            [Option("rune", "Nom de la rune", true)]
            [Autocomplete(typeof(RuneAutocompleteProvider))]
            string name)
        {
            RuneData? runeData = Bot.Instance.Api.Datacenter.RunesData.GetRuneDataByName(name);
            if (runeData is null)
            {
                await ctx.CreateResponseAsync($"Paramètre incorrect\n{Formatter.Italic("Valeur possible :")} {Formatter.InlineCode(Bot.Instance.Api.Datacenter.RunesData.GetAllRuneName())}");
                return;
            }

            double percentRuneExtractable = Math.Round(runeData.GetPercentStatExtractable((int)itemLevel, (int)statAmount), 2);
            if (percentRuneExtractable == -1)
            {
                await ctx.CreateResponseAsync("Une erreur est survenue lors du calcul du % de statistique extractible");
                return;
            }

            DiscordEmbedBuilder embed = EmbedManager.BuildDofusEmbed(DofusEmbedCategory.Tools, "Calculateur d'obtention de runes")
                .WithTitle(runeData.GetFullName());

            if (statAmount == 1)
            {
                embed.WithDescription($"{Emojis.BaRune(runeData.Id)} {Formatter.Bold(percentRuneExtractable.ToString())}% de chance sur un objet de niveau {Formatter.Bold(itemLevel.ToString())}");
            }
            else
            {
                embed.WithDescription($"{Formatter.Bold(percentRuneExtractable.ToString())}% de puissance extractible sur un objet niveau {Formatter.Bold(itemLevel.ToString())} comprenant {Formatter.Bold(statAmount.ToString())} stats");

                if (!runeData.HasPa && !runeData.HasRa)
                {
                    double averageAmountStatExtractable = runeData.GetStatAmountExtractable((int)itemLevel, (int)statAmount, RuneManager.AVERAGE_MULTIPLICATOR);
                    double[] runesAmount = runeData.GetTotalRunesByStatAmontExtractable(averageAmountStatExtractable);

                    embed.AddField("Moy.", $"{Emojis.BaRune(runeData.Id)} {Formatter.Bold(runesAmount[0].ToString())}");
                }
                else
                {
                    double minAmountStatExtractable = runeData.GetStatAmountExtractable((int)itemLevel, (int)statAmount, RuneManager.MIN_MULTIPLICATOR);
                    double[] minRunesAmount = runeData.GetTotalRunesByStatAmontExtractable(minAmountStatExtractable);

                    double averageAmountStatExtractable = runeData.GetStatAmountExtractable((int)itemLevel, (int)statAmount, RuneManager.AVERAGE_MULTIPLICATOR);
                    double[] averageRunesAmount = runeData.GetTotalRunesByStatAmontExtractable(averageAmountStatExtractable);

                    double maxAmountStatExtractable = runeData.GetStatAmountExtractable((int)itemLevel, (int)statAmount, RuneManager.MAX_MULTIPLICATOR);
                    double[] maxRunesAmount = runeData.GetTotalRunesByStatAmontExtractable(maxAmountStatExtractable);

                    embed.AddField("Min.", GetRunesAmountTextField(minRunesAmount, runeData), true);
                    embed.AddField("Moy.", GetRunesAmountTextField(averageRunesAmount, runeData), true);
                    embed.AddField("Max.", GetRunesAmountTextField(maxRunesAmount, runeData), true);

                    if (runeData.HasRa && maxRunesAmount[2] == 1 && minRunesAmount[2] == 0)
                    {
                        double percentRaObtention = Math.Round(runeData.GetPercentToObtain(RuneType.RA, (int)itemLevel, (int)statAmount), 2);
                        embed.AddField("Taux Ra :", $"{Emojis.RaRune(runeData.Id)} {Formatter.Bold(percentRaObtention.ToString())}%");
                    }
                    else if (runeData.HasPa && maxRunesAmount[1] == 1 && minRunesAmount[1] == 0)
                    {
                        double percentPaObtention = Math.Round(runeData.GetPercentToObtain(RuneType.PA, (int)itemLevel, (int)statAmount), 2);
                        embed.AddField("Taux Pa :", $"{Emojis.PaRune(runeData.Id)} {Formatter.Bold(percentPaObtention.ToString())}%");
                    }
                }
            }

            embed.AddField("Source :", "https://forums.jeuxonline.info/sujet/1045383/les-taux-de-brisage");

            await ctx.CreateResponseAsync(embed);
        }

        private static string GetRunesAmountTextField(double[] runesAmount, RuneData rune)
        {
            string result = $"{Emojis.BaRune(rune.Id)} {Formatter.Bold(runesAmount[0].ToString())}";

            if (runesAmount[1] > 0)
            {
                result += $"\n{Emojis.PaRune(rune.Id)} {Formatter.Bold(runesAmount[1].ToString())}";
            }

            if (runesAmount[2] > 0)
            {
                result += $"\n{Emojis.RaRune(rune.Id)} {Formatter.Bold(runesAmount[2].ToString())}";
            }

            return result;
        }
    }
}
