﻿using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record ItemLivingMaxSkinEffect
    : Effect, IEffect
{
    public int Number { get; init; }

    private ItemLivingMaxSkinEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int number)
        : base(id, duration, probability, criteria, effectArea)
    {
        Number = number;
    }

    internal static ItemLivingMaxSkinEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, Number);
    }
}