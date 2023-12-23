using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record RideDetailsEffect : Effect, IEffect<RideDetailsEffect>
{
    public int ItemUuid { get; init; }
    public long UnknownId { get; init; } //Fuck long

    private RideDetailsEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int itemUuid, long unknownId)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        ItemUuid = itemUuid;
        UnknownId = unknownId;
    }

    public static RideDetailsEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param2);
    }

    public Description GetDescription()
    {
        return GetDescription(ItemUuid, UnknownId);
    }
}
