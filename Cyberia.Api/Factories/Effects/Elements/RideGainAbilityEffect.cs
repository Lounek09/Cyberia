using Cyberia.Api.Data.Rides;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record RideGainAbilityEffect : Effect, IEffect
{
    public int RideAbilityId { get; init; }

    private RideGainAbilityEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int rideAbilityId)
        : base(id, duration, probability, criteria, effectArea)
    {
        RideAbilityId = rideAbilityId;
    }

    internal static RideGainAbilityEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public RideAbilityData? GetRideAbilityData()
    {
        return DofusApi.Datacenter.RidesData.GetRideAbilityDataById(RideAbilityId);
    }

    public Description GetDescription()
    {
        var rideAbilityName = DofusApi.Datacenter.RidesData.GetRideAbilityNameById(RideAbilityId);

        return GetDescription(string.Empty, string.Empty, rideAbilityName);
    }
}
