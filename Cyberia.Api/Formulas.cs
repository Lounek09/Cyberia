namespace Cyberia.Api
{
    public static class Formulas
    {
        public static int GetCriticalRate(int nbCrit, int targetCrit, int agility)
        {
            try
            {
                double withoutAgility = targetCrit - nbCrit;
                double withAgility = Math.Floor((targetCrit - nbCrit) * 2.9901 / Math.Log(agility + 12));
                return Convert.ToInt32(Math.Max(2, Math.Min(withoutAgility, withAgility)));
            }
            catch (OverflowException)
            {
                return -1;
            }
        }

        public static int GetAgilityForHalfCriticalRate(int nbCrit, int targetCrit)
        {
            try
            {
                double agility = Math.Exp((targetCrit - nbCrit) * 2.9901 / 3) - 12;

                return Convert.ToInt32(agility);
            }
            catch (OverflowException)
            {
                return -1;
            }
        }

        public static double GetEscapePercent(int agility, int foeAgility)
        {
            double escapePercent = 300 * ((double)agility + 25) / ((double)agility + foeAgility + 50) - 100;
            return Math.Max(0, Math.Min(100, Math.Round(escapePercent, 2)));
        }

        public static int GetAgilityToEscapeForSure(int foeAgility)
        {
            return 25 + 2 * foeAgility;
        }

        public static TimeSpan GetTimePerCraft(int qte, int nbSlot)
        {
            try
            {
                return TimeSpan.FromSeconds((1 + (nbSlot - 1) * 0.15) * qte);
            }
            catch
            {
                return TimeSpan.Zero;
            }
        }
    }
}
