using Cyberia.Api.Data.TTG;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record GiveTTGCardFromFamilyEffect : Effect, IEffect<GiveTTGCardFromFamilyEffect>
{
    public int TTGFamilyId { get; init; }

    private GiveTTGCardFromFamilyEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int ttgFamilyId)
        : base(id, duration, probability, criteria, effectArea)
    {
        TTGFamilyId = ttgFamilyId;
    }

    public static GiveTTGCardFromFamilyEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public TTGFamilyData? GetTTGFamilyData()
    {
        return DofusApi.Datacenter.TTGData.GetTTGFamilyDataById(TTGFamilyId);
    }

    public Description GetDescription()
    {
        var ttgFamilyName = DofusApi.Datacenter.TTGData.GetTTGFamilyNameById(TTGFamilyId);

        return GetDescription(null, null, ttgFamilyName);
    }
}
