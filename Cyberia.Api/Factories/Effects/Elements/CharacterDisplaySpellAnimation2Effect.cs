﻿using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterDisplaySpellAnimation2Effect
    : Effect, IEffect
{
    public int GfxId { get; init; }

    private CharacterDisplaySpellAnimation2Effect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int gfxId)
        : base(id, duration, probability, criteria, effectArea)
    {
        GfxId = gfxId;
    }

    internal static CharacterDisplaySpellAnimation2Effect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, GfxId);
    }
}