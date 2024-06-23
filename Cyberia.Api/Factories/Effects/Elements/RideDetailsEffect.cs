using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record RideDetailsEffect : Effect
{
    public int ItemUuid { get; init; }
    public DateTime ExpirationDate { get; init; }

    private RideDetailsEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int itemUuid, DateTime expirationDate)
        : base(id, duration, probability, criteria, effectArea)
    {
        ItemUuid = itemUuid;
        ExpirationDate = expirationDate;
    }

    internal static RideDetailsEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1, DateTimeOffset.FromUnixTimeMilliseconds(parameters.Param2).UtcDateTime);
    }

    public override Description GetDescription()
    {
        return GetDescription(ItemUuid, ExpirationDate.ToString("dd/MM/yyyy HH:mm"));
    }
}
