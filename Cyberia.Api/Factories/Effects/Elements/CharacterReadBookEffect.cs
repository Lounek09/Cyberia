﻿using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterReadBookEffect
    : Effect, IEffect
{
    public int BookId { get; init; }

    private CharacterReadBookEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int bookId)
        : base(id, duration, probability, criteria, effectArea)
    {
        BookId = bookId;
    }

    internal static CharacterReadBookEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, BookId);
    }
}