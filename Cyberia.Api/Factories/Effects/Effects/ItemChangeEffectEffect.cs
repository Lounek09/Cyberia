﻿using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record ItemChangeEffectEffect : ParameterlessEffect, IEffect<ItemChangeEffectEffect>
{
    private ItemChangeEffectEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
        : base(id, duration, probability, criteria, effectArea)
    {

    }

    public static ItemChangeEffectEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea);
    }
}