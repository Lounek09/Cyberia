using Cyberia.Api.Data.TTG;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record GiveTTGCardFromFamilyEffect
    : Effect, IEffect
{
    public int TTGFamilyId { get; init; }

    private GiveTTGCardFromFamilyEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int ttgFamilyId)
        : base(id, duration, probability, criteria, effectArea)
    {
        TTGFamilyId = ttgFamilyId;
    }

    internal static GiveTTGCardFromFamilyEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public TTGFamilyData? GetTTGFamilyData()
    {
        return DofusApi.Datacenter.TTGData.GetTTGFamilyDataById(TTGFamilyId);
    }

    public Description GetDescription()
    {
        var ttgFamilyName = DofusApi.Datacenter.TTGData.GetTTGFamilyNameById(TTGFamilyId);

        return GetDescription(string.Empty, string.Empty, ttgFamilyName);
    }
}
