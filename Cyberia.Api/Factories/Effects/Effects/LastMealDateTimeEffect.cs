﻿using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record LastMealDateTimeEffect : Effect, IEffect<LastMealDateTimeEffect>
    {
        public DateTime DateTime { get; init; }

        private LastMealDateTimeEffect(int effectId, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea, DateTime dateTime) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            DateTime = dateTime;
        }

        public static LastMealDateTimeEffect Create(int effectId, EffectParameters parameters, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, DateManager.GetDateTimeFromEffectParameters(parameters));
        }

        public Description GetDescription()
        {
            return GetDescription(DateTime.ToString("dd/MM/yyy HH:mm"));
        }
    }
}