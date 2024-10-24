﻿using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record FightSetStateEffect : StateEffect
{
    private FightSetStateEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int stateId)
        : base(id, duration, probability, criteria, effectArea, stateId)
    {

    }

    internal static FightSetStateEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }
}
