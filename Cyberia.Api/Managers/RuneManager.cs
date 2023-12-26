using Cyberia.Api.Data.Items;
using Cyberia.Api.Data.Runes;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Values;

namespace Cyberia.Api.Managers;

public record struct RuneBundle(RuneData RuneData, int BaAmount, int PaAmount, int RaAmount, int RemainingBaPercent);

public static class RuneManager
{
    public const double MIN_MULTIPLICATOR = 0.9;
    public const double AVERAGE_MULTIPLICATOR = 1;
    public const double MAX_MULTIPLICATOR = 1.1;

    public static double GetPercentStatExtractable(RuneData runeData, int itemLvl, int amount)
    {
        var a = Math.Pow(itemLvl, 2);
        var b = Math.Pow(runeData.Weight, 1.25);
        var c = 1.5 * (a / b);
        var percentRuneExtractable = c + (amount - 1) / runeData.Weight * Math.Max(0, 2D / 3 * 100 - c);
        return Math.Min(2D / 3 * 100, percentRuneExtractable);
    }

    public static double GetPercentToObtainRune(RuneData runeData, RuneType type, int itemLvl, int statAmount)
    {
        var multiplicator = GetRequiredStatAmountExtractableToObtainRune(runeData, type) / (GetPercentStatExtractable(runeData, itemLvl, statAmount) / 100) / statAmount;

        if (multiplicator <= MIN_MULTIPLICATOR)
        {
            return 100;
        }

        if (multiplicator >= MAX_MULTIPLICATOR)
        {
            return 0;
        }

        return (multiplicator - MAX_MULTIPLICATOR) / (MIN_MULTIPLICATOR - MAX_MULTIPLICATOR) * 100;
    }

    public static RuneBundle GetRuneBundleFromStat(RuneData runeData, int itemLvl, int statAmount, double? multiplicator = null)
    {
        var bundle = new RuneBundle(runeData, 0, 0, 0, 0);

        var amountExtractable = multiplicator.HasValue
            ? GetStatAmountExtractable(runeData, itemLvl, statAmount, multiplicator.Value)
            : GetStatAmountExtractable(runeData, itemLvl, statAmount, GetRandomMultiplicator());

        var amontBeforeRa = amountExtractable;
        double transitionalRaRate = GetRequiredStatAmountExtractableToObtainRune(runeData, RuneType.RA);
        if (runeData.HasRa && amontBeforeRa > transitionalRaRate)
        {
            for (var i = 0; i < Math.Floor(amontBeforeRa / transitionalRaRate); i++)
            {
                bundle.BaAmount += 4;
                bundle.PaAmount += 2;
                bundle.RaAmount += 1;

                amountExtractable -= transitionalRaRate;
            }
        }

        var amontBeforePa = amountExtractable;
        double transitionalPaRate = GetRequiredStatAmountExtractableToObtainRune(runeData, RuneType.PA);
        if (runeData.HasPa && amontBeforePa > transitionalPaRate)
        {
            for (var i = 0; i < Math.Floor(amontBeforePa / transitionalPaRate); i++)
            {
                bundle.BaAmount += 2;
                bundle.PaAmount += 1;

                amountExtractable -= transitionalPaRate;
            }
        }

        amountExtractable /= GetRequiredStatAmountExtractableToObtainRune(runeData, RuneType.BA);

        var lastBaRune = (int)Math.Floor(amountExtractable);
        bundle.BaAmount += lastBaRune;

        amountExtractable -= lastBaRune;
        bundle.RemainingBaPercent += (int)(Math.Round(amountExtractable, 2) * 100);

        return bundle;
    }

    public static IReadOnlyCollection<RuneBundle> GetRuneBundlesFromItem(ItemData itemData, int qte, double? multiplicator = null)
    {
        var itemstatsData = itemData.GetItemStatsData();
        if (itemstatsData is null)
        {
            return [];
        }

        Dictionary<int, RuneBundle> bundles = [];

        foreach (var effect in itemstatsData.Effects.OfType<IRuneGeneratorEffect>())
        {
            var runeData = effect.GetRuneData();
            if (runeData is null)
            {
                continue;
            }

            for (var i = 0; i < qte; i++)
            {
                var bundle = GetRuneBundleFromStat(runeData, itemData.Level, effect.GetRandomValue(), multiplicator);

                if (bundles.TryGetValue(runeData.Id, out var value))
                {
                    if (value.RemainingBaPercent + bundle.RemainingBaPercent >= 100)
                    {
                        bundle.BaAmount++;
                        bundle.RemainingBaPercent -= 100;
                    }

                    bundles[runeData.Id] = new RuneBundle(runeData,
                        value.BaAmount + bundle.BaAmount,
                        value.PaAmount + bundle.PaAmount,
                        value.RaAmount + bundle.RaAmount,
                        value.RemainingBaPercent + bundle.RemainingBaPercent);
                }
                else
                {
                    bundles.Add(runeData.Id, bundle);
                }
            }
        }

        return bundles.Values;
    }

    private static int GetRequiredStatAmountExtractableToObtainRune(RuneData runeData, RuneType type)
    {
        return type switch
        {
            RuneType.BA => runeData.GetPower(RuneType.BA),
            RuneType.PA => 2 * runeData.GetPower(RuneType.BA) + runeData.GetPower(RuneType.PA),
            RuneType.RA => 4 * runeData.GetPower(RuneType.BA) + 2 * runeData.GetPower(RuneType.PA) + runeData.GetPower(RuneType.RA),
            _ => 0,
        };
    }

    private static double GetRandomMultiplicator()
    {
        return new Random().NextDouble() * (MAX_MULTIPLICATOR - MIN_MULTIPLICATOR) + MIN_MULTIPLICATOR;
    }

    private static double GetStatAmountExtractable(RuneData runeData, int itemLvl, int statAmount, double multiplicator)
    {
        return statAmount * GetPercentStatExtractable(runeData, itemLvl, statAmount) * multiplicator / 100;
    }
}
