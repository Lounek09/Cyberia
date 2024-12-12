using Cyberia.Api.Data.Titles;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record GainTitleEffect : Effect
{
    public int TitleId { get; init; }

    private GainTitleEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, int titleId)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        TitleId = titleId;
    }

    internal static GainTitleEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, dispellable, effectArea, (int)parameters.Param3);
    }

    public TitleData? GetTitleData()
    {
        return DofusApi.Datacenter.TitlesRepository.GetTitleDataById(TitleId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var titleName = DofusApi.Datacenter.TitlesRepository.GetTitleNameById(TitleId, culture);

        return GetDescription(culture, string.Empty, string.Empty, titleName);
    }
}
