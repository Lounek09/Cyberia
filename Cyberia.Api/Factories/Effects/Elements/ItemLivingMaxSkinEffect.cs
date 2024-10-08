﻿using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record ItemLivingMaxSkinEffect : Effect
{
    public int Number { get; init; }

    private ItemLivingMaxSkinEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int number)
        : base(id, duration, probability, criteria, effectArea)
    {
        Number = number;
    }

    internal static ItemLivingMaxSkinEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, Number);
    }
}
