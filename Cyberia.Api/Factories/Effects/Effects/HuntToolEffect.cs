﻿using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record HuntToolEffect : ParameterlessEffect, IEffect<HuntToolEffect>, IRuneGeneratorEffect
{
    public int RuneId { get; init; }

    private HuntToolEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
        : base(id, duration, probability, criteria, effectArea)
    {
        RuneId = 31;
    }

    public int GetRandomValue()
    {
        return 1;
    }

    public static HuntToolEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea);
    }
}