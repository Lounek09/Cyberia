﻿using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record PetsLastMealEffect
    : Effect, IEffect
{
    public DateTime DateTime { get; init; }

    private PetsLastMealEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, DateTime dateTime)
        : base(id, duration, probability, criteria, effectArea)
    {
        DateTime = dateTime;
    }

    internal static PetsLastMealEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, DateManager.CreateDateTimeFromEffectParameters(parameters));
    }

    public Description GetDescription()
    {
        return GetDescription(DateTime.ToString("dd/MM/yyy HH:mm"));
    }
}