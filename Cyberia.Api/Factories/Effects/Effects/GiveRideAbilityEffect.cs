using Cyberia.Api.Data;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record GiveRideAbilityEffect : Effect, IEffect<GiveRideAbilityEffect>
{
    public int RideAbilityId { get; init; }

    private GiveRideAbilityEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int rideAbilityId)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        RideAbilityId = rideAbilityId;
    }

    public static GiveRideAbilityEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public RideAbilityData? GetRideAbilityData()
    {
        return DofusApi.Datacenter.RidesData.GetRideAbilityDataById(RideAbilityId);
    }

    public Description GetDescription()
    {
        var rideAbilityName = DofusApi.Datacenter.RidesData.GetRideAbilityNameById(RideAbilityId);

        return GetDescription(null, null, rideAbilityName);
    }
}
