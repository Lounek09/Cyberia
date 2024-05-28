namespace Cyberia.Api;

public static class Formulas
{
    public static int GetCriticalRate(int crit, int targetCrit, int agility)
    {
        double withoutAgility = targetCrit - crit;
        var withAgility = Math.Floor((targetCrit - crit) * 2.9901 / Math.Log(agility + 12));

        return Convert.ToInt32(Math.Max(2, Math.Min(withoutAgility, withAgility)));

    }

    public static int GetAgilityForHalfCriticalRate(int crit, int targetCrit)
    {
        var agility = Math.Exp((targetCrit - crit) * 2.9901 / 3) - 12;

        return Convert.ToInt32(agility);
    }

    public static double GetEscapePercent(int agility, int foeAgility)
    {
        var escapePercent = 300 * ((double)agility + 25) / ((double)agility + foeAgility + 50) - 100;

        return Math.Max(0, Math.Min(100, Math.Round(escapePercent, 2)));
    }

    public static int GetAgilityToEscapeForSure(int foeAgility)
    {
        return 25 + 2 * foeAgility;
    }

    public static TimeSpan GetTimePerCraft(int quantity, int slot)
    {
        return TimeSpan.FromSeconds((1 + (slot - 1) * 0.15) * quantity);
    }
}
