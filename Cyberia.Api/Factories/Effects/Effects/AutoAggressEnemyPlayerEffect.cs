﻿using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record AutoAggressEnemyPlayerEffect : Effect, IEffect<AutoAggressEnemyPlayerEffect>
{
    public int Distance { get; init; }

    private AutoAggressEnemyPlayerEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int distance)
        : base(id, duration, probability, criteria, effectArea)
    {
        Distance = distance;
    }

    public static AutoAggressEnemyPlayerEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1);
    }

    public Description GetDescription()
    {
        return GetDescription(Distance);
    }
}