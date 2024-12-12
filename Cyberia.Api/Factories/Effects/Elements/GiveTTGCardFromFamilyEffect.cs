using Cyberia.Api.Data.TTG;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record GiveTTGCardFromFamilyEffect : Effect
{
    public int TTGFamilyId { get; init; }

    private GiveTTGCardFromFamilyEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, int ttgFamilyId)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        TTGFamilyId = ttgFamilyId;
    }

    internal static GiveTTGCardFromFamilyEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, dispellable, effectArea, (int)parameters.Param3);
    }

    public TTGFamilyData? GetTTGFamilyData()
    {
        return DofusApi.Datacenter.TTGRepository.GetTTGFamilyDataById(TTGFamilyId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var ttgFamilyName = DofusApi.Datacenter.TTGRepository.GetTTGFamilyNameById(TTGFamilyId, culture);

        return GetDescription(culture, string.Empty, string.Empty, ttgFamilyName);
    }
}
