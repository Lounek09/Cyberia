namespace Cyberia.Api.Utils;

/// <summary>
/// Provides utilities for various game-related calculations.
/// </summary>
public static class Formulas
{
    /// <summary>
    /// Calculates the critical hit rate based on character stats.
    /// </summary>
    /// <param name="baseRate">The base critical hit rate of the spell or weapon.</param>
    /// <param name="criticalHit">The character's number of critical hits.</param>
    /// <param name="agility">The character's agility stat.</param>
    /// <returns>The actual critical hit rate (minimum 2).</returns>
    public static int GetCriticalHitRate(int baseRate, int criticalHit, int agility)
    {
        var calculatedRate = baseRate - criticalHit;
        var agilityCalculatedRate = Math.Floor(calculatedRate * 2.9901 / Math.Log(agility + 12));
        var effectiveRate = Math.Min(calculatedRate, agilityCalculatedRate);

        if (effectiveRate < 2)
        {
            return 2;
        }

        if (effectiveRate > int.MaxValue)
        {
            return int.MaxValue;
        }

        return (int)effectiveRate;
    }

    /// <summary>
    /// Calculates the agility required to achieve a critical hit rate of 1/2.
    /// </summary>
    /// <param name="baseRate">The base critical hit rate of the spell or weapon.</param>
    /// <param name="criticalHit">The character's number of critical hits.</param>
    /// <returns>The agility required to achieve a critical hit rate of 1/2.</returns>
    public static int GetAgilityForOptimalCriticalHitRate(int baseRate, int criticalHit)
    {
        var agility = Math.Exp((baseRate - criticalHit) * 2.9901 / 3) - 12;

        if (agility < 0)
        {
            return 0;
        }

        if (agility > int.MaxValue)
        {
            return int.MaxValue;
        }

        return (int)agility;
    }

    /// <summary>
    /// Calculates the escape percentage chance based on character and enemy agility.
    /// </summary>
    /// <param name="agility">The character's agility stat.</param>
    /// <param name="enemyAgility">The enemy's agility stat.</param>
    /// <returns>The escape percentage chance (0-100%).</returns>
    public static double GetEscapePercentProbability(int agility, int enemyAgility)
    {
        var escapePercent = 300 * ((double)agility + 25) / ((double)agility + enemyAgility + 50) - 100;

        if (escapePercent < 0)
        {
            return 0;
        }

        if (escapePercent > 100)
        {
            return 100;
        }

        return Math.Round(escapePercent, 2);
    }

    /// <summary>
    /// Calculates the agility needed to guarantee escape from an enemy.
    /// </summary>
    /// <param name="enemyAgility">The enemy's agility stat.</param>
    /// <returns>The agility needed to guarantee escape.</returns>
    public static int GetAgilityForGuaranteedEscape(int enemyAgility)
    {
        var agility = 25 + 2 * enemyAgility;

        if (agility < 0)
        {
            return 0;
        }

        return agility;
    }

    /// <summary>
    /// Calculates the time required to craft a given quantity of items based on the number of ingredients.
    /// </summary>
    /// <param name="quantity">The quantity of items to craft.</param>
    /// <param name="ingredientCount">The number of ingredients required.</param>
    /// <returns>The time required to craft the items.</returns>
    public static TimeSpan GetTimePerCraft(int quantity, int ingredientCount)
    {
        var additionalTime = 0.15 * (ingredientCount - 1);
        return TimeSpan.FromSeconds((1 + additionalTime) * quantity);
    }
}
