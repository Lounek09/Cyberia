using Cyberia.Api.Data;

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

        public static int GetPower(this RuneData runeData, RuneType type)
        {
            return type switch
            {
                RuneType.BA => runeData.Power,
                RuneType.PA => runeData.Power * 3 + (runeData.Id == 4 ? 1 : 0),
                RuneType.RA => runeData.Power * 10,
                _ => 0,
            };
        }

        public static double GetPercentStatExtractable(this RuneData runeData, int itemLvl, int statAmount)
        {
            double a = Math.Pow(itemLvl, 2);
            double b = Math.Pow(runeData.Weight, 1.25);
            double c = 1.5 * (a / b);
            double percentRuneExtractable = c + (statAmount - 1) / runeData.Weight * Math.Max(0, 2D / 3 * 100 - c);
            return Math.Min(2D / 3 * 100, percentRuneExtractable);
        }

        public static double GetStatAmountExtractable(this RuneData runeData, int itemLvl, int statAmount, double multiplicator)
        {
            return statAmount * runeData.GetPercentStatExtractable(itemLvl, statAmount) * multiplicator / 100;
        }

        public static double GetStatAmountExtractable(this RuneData runeData, int itemLvl, int statAmount)
        {
            return runeData.GetStatAmountExtractable(itemLvl, statAmount, GetRandomMultiplicator());
        }

        public static int GetStatAmountExtractableToObtain(this RuneData runeData, RuneType type)
        {
            return type switch
            {
                RuneType.BA => runeData.GetPower(RuneType.BA),
                RuneType.PA => 2 * runeData.GetPower(RuneType.BA) + runeData.GetPower(RuneType.PA),
                RuneType.RA => 4 * runeData.GetPower(RuneType.BA) + 2 * runeData.GetPower(RuneType.PA) + runeData.GetPower(RuneType.RA),
                _ => 0,
            };
        }

        public static double GetPercentToObtain(this RuneData runeData, RuneType type, int itemLvl, int statAmount)
        {
            double multiplicator = runeData.GetStatAmountExtractableToObtain(type) / (runeData.GetPercentStatExtractable(itemLvl, statAmount) / 100) / statAmount;

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

        public static double[] GetTotalRunesByStatAmontExtractable(this RuneData runeData, double amountExtractable)
        {
            double[] runesAmont = [ 0, 0, 0 ];

            double amontBeforeRa = amountExtractable;
            double transitionalRaRate = runeData.GetStatAmountExtractableToObtain(RuneType.RA);
            if (runeData.HasRa && amontBeforeRa > transitionalRaRate)
            {
                for (int i = 0; i < Math.Floor(amontBeforeRa / transitionalRaRate); i++)
                {
                    runesAmont[0] += 4;
                    runesAmont[1] += 2;
                    runesAmont[2] += 1;

                    amountExtractable -= transitionalRaRate;
                }
            }

            double amontBeforePa = amountExtractable;
            double transitionalPaRate = runeData.GetStatAmountExtractableToObtain(RuneType.PA);
            if (runeData.HasPa && amontBeforePa > transitionalPaRate)
            {
                for (int i = 0; i < Math.Floor(amontBeforePa / transitionalPaRate); i++)
                {
                    runesAmont[0] += 2;
                    runesAmont[1] += 1;

                    amountExtractable -= transitionalPaRate;
                }
            }

            amountExtractable /= runeData.GetStatAmountExtractableToObtain(RuneType.BA);

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
