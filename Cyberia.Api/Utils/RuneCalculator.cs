using Cyberia.Api.Data.Items;
using Cyberia.Api.Data.Runes;
using Cyberia.Api.Enums;
using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Utils;

/// <summary>
/// Represents a bundle of runes extracted from items, with quantities of different rune types.
/// </summary>
public record struct RuneBundle(RuneData RuneData, int BaQuantity, int PaQuantity, int RaQuantity, double RemainingBaPercent);

/// <summary>
/// Provides calculations for rune extraction mechanics from items.
/// </summary>
public static class RuneCalculator
{
    public const string Source = "https://forums.jeuxonline.info/sujet/1045383/les-taux-de-brisage";

    /// <summary>
    /// Minimum multiplier used in rune extraction probability calculations.
    /// </summary>
    public const double MinMultiplicator = 0.9;

    /// <summary>
    /// Average multiplier used in rune extraction probability calculations.
    /// </summary>
    public const double AverageMultiplicator = 1;

    /// <summary>
    /// Maximum multiplier used in rune extraction probability calculations.
    /// </summary>
    public const double MaxMultiplicator = 1.1;

    // Maximum percentage of a stat that can be extracted (2/3 * 100 = 66.67%)
    private const double MaxExtractablePercent = 2D / 3D * 100;

    /// <summary>
    /// Calculates the percentage of a stat that can be extracted from an item.
    /// </summary>
    /// <param name="runeData">The rune data for the stat</param>
    /// <param name="itemLevel">The item's level</param>
    /// <param name="statAmount">The amount of the stat on the item</param>
    /// <returns>The percentage of the stat that can be extracted</returns>
    public static double GetPercentStatExtractable(RuneData runeData, int itemLevel, int statAmount)
    {
        var a = Math.Pow(itemLevel, 2);
        var b = Math.Pow(runeData.Weight, 1.25);
        var c = 1.5 * (a / b);
        var d = c + (statAmount - 1) / runeData.Weight * Math.Max(0, MaxExtractablePercent - c);

        return Math.Min(MaxExtractablePercent, d);
    }

    /// <summary>
    /// Calculates the percentage chance to obtain a specific rune type.
    /// </summary>
    /// <param name="runeData">The rune data to obtain</param>
    /// <param name="type">The type of rune (BA, PA, RA)</param>
    /// <param name="itemLevel">The item's level</param>
    /// <param name="statAmount">The amount of the stat on the item</param>
    /// <returns>The percentage chance to obtain the rune</returns>
    public static double GetPercentChanceToObtainRune(RuneData runeData, RuneType type, int itemLevel, int statAmount)
    {
        var requiredAmount = GetRequiredStatAmountExtractableToObtainRune(runeData, type);
        var extractableCoeff = GetPercentStatExtractable(runeData, itemLevel, statAmount) / 100;
        var multiplicator = requiredAmount / extractableCoeff / statAmount;

        if (multiplicator <= MinMultiplicator)
        {
            return 100;
        }

        if (multiplicator >= MaxMultiplicator)
        {
            return 0;
        }

        return (multiplicator - MaxMultiplicator) / (MinMultiplicator - MaxMultiplicator) * 100;
    }

    /// <summary>
    /// Calculates the runes obtainable from a specific stat on an item.
    /// </summary>
    /// <param name="runeData">The rune data to obtain</param>
    /// <param name="itemLevel">The item's level</param>
    /// <param name="statAmount">The amount of the stat on the item</param>
    /// <param name="multiplicator">Optional custom multiplier (random if not provided)</param>
    /// <returns>A <see cref="RuneBundle"/> containing the quantity of each rune type</returns>
    public static RuneBundle GetRuneBundleFromStat(RuneData runeData, int itemLevel, int statAmount, double? multiplicator = null)
    {
        RuneBundle bundle = new(runeData, 0, 0, 0, 0);
        var actualMultiplier = multiplicator ?? GetRandomMultiplicator();
        var amountExtractable = statAmount * GetPercentStatExtractable(runeData, itemLevel, statAmount) * actualMultiplier / 100;

        if (runeData.RaRuneItemId is not null)
        {
            var amountBeforeRa = amountExtractable;
            var raThreshold = GetRequiredStatAmountExtractableToObtainRune(runeData, RuneType.RA);

            if (amountBeforeRa > raThreshold)
            {
                var raCount = (int)Math.Floor(amountBeforeRa / raThreshold);
                bundle.BaQuantity += 4 * raCount;
                bundle.PaQuantity += 2 * raCount;
                bundle.RaQuantity += raCount;
                amountExtractable -= raThreshold * raCount;
            }
        }

        if (runeData.PaRuneItemId is not null)
        {
            var amountBeforePa = amountExtractable;
            var paThreshold = GetRequiredStatAmountExtractableToObtainRune(runeData, RuneType.PA);

            if (amountBeforePa > paThreshold)
            {
                var paCount = (int)Math.Floor(amountBeforePa / paThreshold);
                bundle.BaQuantity += 2 * paCount;
                bundle.PaQuantity += paCount;
                amountExtractable -= paThreshold * paCount;
            }
        }

        var baThreshold = GetRequiredStatAmountExtractableToObtainRune(runeData, RuneType.BA);
        amountExtractable /= baThreshold;

        var baCount = (int)Math.Floor(amountExtractable);
        bundle.BaQuantity += baCount;

        bundle.RemainingBaPercent = (amountExtractable - baCount) * 100;

        return bundle;
    }

    /// <summary>
    /// Calculates all possible runes obtainable from an item from the specified quantity.
    /// </summary>
    /// <param name="itemData">The item data</param>
    /// <param name="quantity">The quantity of items</param>
    /// <param name="multiplicator">Optional custom multiplier (random if not provided)</param>
    /// <returns>A list of <see cref="RuneBundle"/> containing the quantity of each rune type</returns>
    public static IReadOnlyCollection<RuneBundle> GetRuneBundlesFromItem(ItemData itemData, int quantity, double? multiplicator = null)
    {
        var itemstatsData = itemData.GetItemStatsData();
        if (itemstatsData is null)
        {
            return [];
        }

        Dictionary<int, RuneBundle> bundles = [];

        foreach (var effect in itemstatsData.Effects)
        {
            if (effect is not IRuneGeneratorEffect runeGeneratorEffect)
            {
                continue;
            }

            var runeData = runeGeneratorEffect.GetRuneData();
            if (runeData is null)
            {
                continue;
            }

            for (var i = 0; i < quantity; i++)
            {
                var bundle = GetRuneBundleFromStat(runeData, itemData.Level, runeGeneratorEffect.GetRandomValue(), multiplicator);

                if (Random.Shared.NextDouble() * 100 < bundle.RemainingBaPercent)
                {
                    bundle.BaQuantity++;
                }

                bundle.RemainingBaPercent = 0;

                if (bundles.TryGetValue(runeData.Id, out var existingBundle))
                {
                    bundles[runeData.Id] = existingBundle with
                    {
                        BaQuantity = existingBundle.BaQuantity + bundle.BaQuantity,
                        PaQuantity = existingBundle.PaQuantity + bundle.PaQuantity,
                        RaQuantity = existingBundle.RaQuantity + bundle.RaQuantity,
                    };
                }
                else
                {
                    bundles.Add(runeData.Id, bundle);
                }
            }
        }

        return bundles.Values;
    }

    /// <summary>
    /// Calculates the required amount of a stat to obtain a specific type of rune.
    /// </summary>
    /// <param name="runeData">The rune data to obtain</param>
    /// <param name="type">The rune type wanted</param>
    /// <returns>The amount required for that rune type</returns>
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

    /// <summary>
    /// Generates a random multiplier between the minimum and maximum values.
    /// </summary>
    /// <returns>A random multiplier value</returns>
    private static double GetRandomMultiplicator()
    {
        return Random.Shared.NextDouble() * (MaxMultiplicator - MinMultiplicator) + MinMultiplicator;
    }
}
