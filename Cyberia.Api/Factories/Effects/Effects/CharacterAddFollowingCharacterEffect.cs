﻿using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterAddFollowingCharacterEffect : Effect, IEffect<CharacterAddFollowingCharacterEffect>
{
    public int SpriteId { get; init; }

    private CharacterAddFollowingCharacterEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int spriteId)
        : base(id, duration, probability, criteria, effectArea)
    {
        SpriteId = spriteId;
    }

    public static CharacterAddFollowingCharacterEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(SpriteId);
    }
}