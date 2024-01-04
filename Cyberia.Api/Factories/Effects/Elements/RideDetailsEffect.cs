using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record RideDetailsEffect
    : Effect, IEffect
{
    public int ItemUuid { get; init; }
    public long UnknownId { get; init; } //Fuck long

    private RideDetailsEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int itemUuid, long unknownId)
        : base(id, duration, probability, criteria, effectArea)
    {
        ItemUuid = itemUuid;
        UnknownId = unknownId;
    }

    internal static RideDetailsEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param2);
    }

    public Description GetDescription()
    {
        return GetDescription(ItemUuid, UnknownId);
    }
}
