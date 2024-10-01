namespace Cyberia.Api;

public static class Formulas
{
    public static int GetCriticalRate(int crit, int targetRate, int agility)
    {
        double withoutAgility = targetRate - crit;
        var withAgility = Math.Floor((targetRate - crit) * 2.9901 / Math.Log(agility + 12));

        var critRate = Math.Max(2, Math.Min(withoutAgility, withAgility));

        return critRate > int.MaxValue ? int.MaxValue : Convert.ToInt32(critRate);

    }

    public static int GetAgilityForHalfCriticalRate(int crit, int targetCrit)
    {
        var agility = Math.Exp((targetCrit - crit) * 2.9901 / 3) - 12;

        return agility > int.MaxValue ? int.MaxValue : Convert.ToInt32(agility);
    }

    public static double GetEscapePercent(int agility, int enemyAgility)
    {
        var escapePercent = 300 * ((double)agility + 25) / ((double)agility + enemyAgility + 50) - 100;

        return Math.Max(0, Math.Min(100, Math.Round(escapePercent, 2)));
    }

    public static int GetAgilityToEscapeForSure(int enemyAgility)
    {
        var agility = 25 + 2D * enemyAgility;

        return agility > int.MaxValue ? int.MaxValue : Convert.ToInt32(agility);
    }

    public static TimeSpan GetTimePerCraft(int quantity, int slot)
    {
        return TimeSpan.FromSeconds((1 + (slot - 1) * 0.15) * quantity);
    }
}
