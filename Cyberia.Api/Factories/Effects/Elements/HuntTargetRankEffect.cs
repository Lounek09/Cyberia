﻿using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record HuntTargetRankEffect : Effect
{
    public int Rank { get; init; }

    private HuntTargetRankEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int rank)
        : base(id, duration, probability, criteria, effectArea)
    {
        Rank = rank;
    }

    internal static HuntTargetRankEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, Rank);
    }
}
