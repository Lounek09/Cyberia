namespace Salamandra.Utils
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

        public static int GetAgilityFor1On2CriticalRate(int nbCrit, int targetCrit)
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

        public static double GetEscapePercent(double agility, double foesAgility)
        {
            try
            {
                double escapePercent = 300 * ((agility + 25) / (agility + foesAgility + 50)) - 100;
                return Math.Max(0, Math.Min(100, Math.Round(escapePercent, 2)));
            }
            catch
            {
                return -1;
            }
        }

        public static double GetPercentRuneExtractable(int lvl, double weightStat, int amont)
        {
            try
            {
                double a = Math.Pow(lvl, 2);
                double b = Math.Pow(weightStat, 1.25);
                double c = 1.5 * (a / b);
                double percentRuneExtractable = c + (amont - 1) / weightStat * Math.Max(0, 66.66 - c);
                return Math.Min(66.66, Math.Round(percentRuneExtractable, 2));
            }
            catch
            {
                return -1;
            }
        }

        /*public static double[] GetAmontRuneByAmontExtractable(double amontExtractable, Rune rune)
        {
            double[] runesAmont = { 0, 0, 0, 0 };

            double amontBeforeRa = amontExtractable;
            double transitionalRaRate = 4 * rune.GetBaPower() + 2 * rune.GetPaPower() + rune.GetRaPower();
            if (rune.HasRa && amontBeforeRa > transitionalRaRate)
            {
                for (int i = 0; i < Math.Floor(amontBeforeRa / transitionalRaRate); i++)
                {
                    runesAmont[0] += 4;
                    runesAmont[1] += 2;
                    runesAmont[2] += 1;

                    amontExtractable -= transitionalRaRate;
                }
            }

            double amontBeforePa = amontExtractable;
            double transitionalPaRate = 2 * rune.GetBaPower() + rune.GetPaPower();
            if (rune.HasPa && amontBeforePa > transitionalPaRate)
            {
                for (int i = 0; i < Math.Floor(amontBeforePa / transitionalPaRate); i++)
                {
                    runesAmont[0] += 2;
                    runesAmont[1] += 1;

                    amontExtractable -= transitionalPaRate;
                }
            }

            amontExtractable *= 1.0 / rune.GetBaPower();

            double lastBaRune = Math.Floor(amontExtractable);
            runesAmont[0] += lastBaRune;

            amontExtractable -= lastBaRune;
            runesAmont[3] = Math.Round(amontExtractable * 100, 2);

            return runesAmont;
        }*/

        public static TimeSpan GetTimeForMultipleCraft(int qte, int nbSlot)
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
