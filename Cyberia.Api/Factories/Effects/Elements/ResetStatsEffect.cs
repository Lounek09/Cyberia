﻿using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Factories.Effects;

public sealed record ResetStatsEffect : ParameterlessEffect
{
    private ResetStatsEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
        : base(id, duration, probability, criteria, effectArea)
    {

    }

    internal static ResetStatsEffect Create(int effectId, EffectParameters _, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea);
    }
}