using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Managers
{
    public enum RuneType
    {
        BA,
        PA,
        RA
    }

    public static class RuneManager
    {
        public const double MIN_MULTIPLICATOR = 0.9;
        public const double AVERAGE_MULTIPLICATOR = 1;
        public const double MAX_MULTIPLICATOR = 1.1;

        public static int GetPower(this Rune rune, RuneType type)
        {
            return type switch
            {
                RuneType.BA => rune.Power,
                RuneType.PA => rune.Power * 3 + (rune.Name.Equals("Vi") ? 1 : 0),
                RuneType.RA => rune.Power * 10,
                _ => 0,
            };
        }

        public static double GetPercentStatExtractable(this Rune rune, int itemLvl, int statAmount)
        {
            double a = Math.Pow(itemLvl, 2);
            double b = Math.Pow(rune.Weight, 1.25);
            double c = 1.5 * (a / b);
            double percentRuneExtractable = c + (statAmount - 1) / rune.Weight * Math.Max(0, 2D / 3 * 100 - c);
            return Math.Min(2D / 3 * 100, percentRuneExtractable);
        }

        public static double GetStatAmountExtractable(this Rune rune, int itemLvl, int statAmount, double multiplicator)
        {
            return statAmount * rune.GetPercentStatExtractable(itemLvl, statAmount) * multiplicator / 100;
        }

        public static double GetStatAmountExtractable(this Rune rune, int itemLvl, int statAmount)
        {
            return rune.GetStatAmountExtractable(itemLvl, statAmount, GetRandomMultiplicator());
        }

        public static int GetStatAmountExtractableToObtain(this Rune rune, RuneType type)
        {
            return type switch
            {
                RuneType.BA => rune.GetPower(RuneType.BA),
                RuneType.PA => 2 * rune.GetPower(RuneType.BA) + rune.GetPower(RuneType.PA),
                RuneType.RA => 4 * rune.GetPower(RuneType.BA) + 2 * rune.GetPower(RuneType.PA) + rune.GetPower(RuneType.RA),
                _ => 0,
            };
        }

        public static double GetPercentToObtain(this Rune rune, RuneType type, int itemLvl, int statAmount)
        {
            double multiplicator = rune.GetStatAmountExtractableToObtain(type) / (rune.GetPercentStatExtractable(itemLvl, statAmount) / 100) / statAmount;

            if (multiplicator <= MIN_MULTIPLICATOR)
                return 100;

            if (multiplicator >= MAX_MULTIPLICATOR)
                return 0;

            return (multiplicator - MAX_MULTIPLICATOR) / (MIN_MULTIPLICATOR - MAX_MULTIPLICATOR) * 100;
        }

        public static double[] GetTotalRunesByStatAmontExtractable(this Rune rune, double amountExtractable)
        {
            double[] runesAmont = { 0, 0, 0 };

            double amontBeforeRa = amountExtractable;
            double transitionalRaRate = rune.GetStatAmountExtractableToObtain(RuneType.RA);
            if (rune.HasRa && amontBeforeRa > transitionalRaRate)
                for (int i = 0; i < Math.Floor(amontBeforeRa / transitionalRaRate); i++)
                {
                    runesAmont[0] += 4;
                    runesAmont[1] += 2;
                    runesAmont[2] += 1;

                    amountExtractable -= transitionalRaRate;
                }

            double amontBeforePa = amountExtractable;
            double transitionalPaRate = rune.GetStatAmountExtractableToObtain(RuneType.PA);
            if (rune.HasPa && amontBeforePa > transitionalPaRate)
                for (int i = 0; i < Math.Floor(amontBeforePa / transitionalPaRate); i++)
                {
                    runesAmont[0] += 2;
                    runesAmont[1] += 1;

                    amountExtractable -= transitionalPaRate;
                }

            amountExtractable /= rune.GetStatAmountExtractableToObtain(RuneType.BA);

            double lastBaRune = Math.Floor(amountExtractable);
            runesAmont[0] += lastBaRune;

            amountExtractable -= lastBaRune;
            runesAmont[0] += Math.Round(amountExtractable, 2);

            return runesAmont;
        }

        private static double GetRandomMultiplicator()
        {
            return Random.Shared.NextDouble() * (MAX_MULTIPLICATOR - MIN_MULTIPLICATOR) + MIN_MULTIPLICATOR;
        }
    }
}
