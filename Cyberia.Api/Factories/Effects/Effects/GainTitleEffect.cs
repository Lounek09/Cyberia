using Cyberia.Api.Data.Titles;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record GainTitleEffect : Effect, IEffect<GainTitleEffect>
{
    public int TitleId { get; init; }

    private GainTitleEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int titleId)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        TitleId = titleId;
    }

    public static GainTitleEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
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

        return GetDescription(null, null, titleName);
    }
}
