using Cyberia.Api.Data.TTG;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Interfaces;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record GiveTTGCardFromFamilyEffect : Effect, ITTGFamilyEffect
{
    public int TTGFamilyId { get; init; }

    private GiveTTGCardFromFamilyEffect(int id, int ttgFamilyId)
        : base(id)
    {
        TTGFamilyId = ttgFamilyId;
    }

    internal static GiveTTGCardFromFamilyEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
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
