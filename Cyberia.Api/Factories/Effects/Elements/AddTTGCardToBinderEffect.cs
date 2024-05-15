using Cyberia.Api.Data.TTG;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record AddTTGCardToBinderEffect : Effect, IEffect
{
    public int TTGCardId { get; init; }

    private AddTTGCardToBinderEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int ttgCardId)
        : base(id, duration, probability, criteria, effectArea)
    {
        TTGCardId = ttgCardId;
    }

    internal static AddTTGCardToBinderEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public TTGCardData? GetTTGCardData()
    {
        return DofusApi.Datacenter.TTGRepository.GetTTGCardDataById(TTGCardId);
    }

    public Description GetDescription()
    {
        var ttgCard = GetTTGCardData();
        var ttgEntityName = ttgCard is null ? $"{nameof(TTGCardData)} {PatternDecoder.Description(Resources.Unknown_Data, TTGCardId)}" : DofusApi.Datacenter.TTGRepository.GetTTGEntityNameById(ttgCard.TTGEntityId);

        return GetDescription(string.Empty, string.Empty, ttgEntityName);
    }
}
