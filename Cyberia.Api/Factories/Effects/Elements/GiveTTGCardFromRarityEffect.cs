﻿using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;
using Cyberia.Api.Values;

namespace Cyberia.Api.Factories.Effects;

public sealed record GiveTTGCardFromRarityEffect
    : Effect, IEffect
{
    public TTGCardRarity Rarity { get; init; }

    private GiveTTGCardFromRarityEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, TTGCardRarity rarity)
        : base(id, duration, probability, criteria, effectArea)
    {
        Rarity = rarity;
    }

    internal static GiveTTGCardFromRarityEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (TTGCardRarity)parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, Rarity.GetDescription());
    }
}