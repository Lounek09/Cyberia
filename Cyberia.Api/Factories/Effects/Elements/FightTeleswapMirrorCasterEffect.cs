﻿using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record FightTeleswapMirrorCasterEffect
    : ParameterlessEffect, IEffect
{
    private FightTeleswapMirrorCasterEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
        : base(id, duration, probability, criteria, effectArea)
    {

    }

    internal static FightTeleswapMirrorCasterEffect Create(int effectId, EffectParameters _, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea);
    }
}