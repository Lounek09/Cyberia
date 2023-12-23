using Cyberia.Api.Data.TTG;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record AddTTGCardToBinderEffect : Effect, IEffect<AddTTGCardToBinderEffect>
{
    public int TTGCardId { get; init; }

    private AddTTGCardToBinderEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int ttgCardId)
        : base(id, duration, probability, criteria, effectArea)
    {
        TTGCardId = ttgCardId;
    }

    public static AddTTGCardToBinderEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public TTGCardData? GetTTGCardData()
    {
        return DofusApi.Datacenter.TTGData.GetTTGCardDataById(TTGCardId);
    }

    public Description GetDescription()
    {
        var ttgCard = GetTTGCardData();
        var ttgEntityName = ttgCard is null ? $"{nameof(TTGCardData)} {PatternDecoder.Description(Resources.Unknown_Data, TTGCardId)}" : DofusApi.Datacenter.TTGData.GetTTGEntityNameById(ttgCard.TTGEntityId);

        return GetDescription(null, null, ttgEntityName);
    }
}
