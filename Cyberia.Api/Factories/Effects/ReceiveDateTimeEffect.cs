using Cyberia.Api.Managers;

using System;

namespace Cyberia.Api.Factories.Effects
{
    public sealed class ReceivedOnDateTimeEffect : BasicEffect
    {
        public int Year { get; init; }
        public int Month { get; init; }
        public int Day { get; init; }
        public int Hour { get; init; }
        public int Minute { get; init; }

        public ReceivedOnDateTimeEffect(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area) : 
            base(effectId, parameters, duration, probability, criteria, area)
        {
            Year = parameters.Param1;
            Month = (int)Math.Floor(parameters.Param2 / 100D) + 1;
            Day = parameters.Param2 - (Month - 1) * 100;
            Hour = (int)Math.Floor(parameters.Param3 / 100D);
            Minute = parameters.Param3 - Hour * 100;
        }

        public static new ReceivedOnDateTimeEffect Create(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area)
        {
            return new(effectId, parameters, duration, probability, criteria, area);
        }

        public override string GetDescription()
        {
            if (Year == -1)
                return GetDescriptionFromParameters("Lié au compte");

            return GetDescriptionFromParameters($"{Day.ToString().PadLeft(2, '0')}/{Month.ToString().PadLeft(2, '0')}/{Year.ToString().PadLeft(2, '0')} {Hour.ToString().PadLeft(2, '0')}:{Minute.ToString().PadLeft(2, '0')}");
        }
    }
}
