﻿using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterGainXpFromLevelEffect : Effect, IEffect<CharacterGainXpFromLevelEffect>
{
    public int Level { get; init; }
    public int RemainingPercent { get; init; }

    private CharacterGainXpFromLevelEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int level, int remainingPercent)
        : base(id, duration, probability, criteria, effectArea)
    {
        Level = level;
        RemainingPercent = remainingPercent;
    }

    public static CharacterGainXpFromLevelEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param2);
    }

    public Description GetDescription()
    {
        return GetDescription(Level, RemainingPercent);
    }
}