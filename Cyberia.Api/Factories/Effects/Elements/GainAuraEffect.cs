﻿using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record GainAuraEffect : Effect
{
    public int AuraId { get; init; }

    private GainAuraEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int auraId)
        : base(id, duration, probability, criteria, effectArea)
    {
        AuraId = auraId;
    }

    internal static GainAuraEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, AuraId);
    }
}
