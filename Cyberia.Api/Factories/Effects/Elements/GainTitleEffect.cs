﻿using Cyberia.Api.Data.Titles;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record GainTitleEffect
    : Effect, IEffect
{
    public int TitleId { get; init; }

    private GainTitleEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int titleId)
        : base(id, duration, probability, criteria, effectArea)
    {
        TitleId = titleId;
    }

    internal static GainTitleEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public TitleData? GetTitleData()
    {
        return DofusApi.Datacenter.TitlesData.GetTitleDataById(TitleId);
    }

    public Description GetDescription()
    {
        var titleName = DofusApi.Datacenter.TitlesData.GetTitleNameById(TitleId);

        return GetDescription(string.Empty, string.Empty, titleName);
    }
}