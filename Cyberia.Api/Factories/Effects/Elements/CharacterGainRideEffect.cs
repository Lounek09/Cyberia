using Cyberia.Api.Data.Rides;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterGainRideEffect : Effect, IEffect
{
    public int RideId { get; init; }
    public int RideAbilityId { get; init; }
    public bool Infertile { get; init; }

    private CharacterGainRideEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int rideId, int rideAbilityId, bool infertile)
        : base(id, duration, probability, criteria, effectArea)
    {
        RideId = rideId;
        RideAbilityId = rideAbilityId;
        Infertile = infertile;
    }

    internal static CharacterGainRideEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3, (int)parameters.Param2, parameters.Param1 == 1);
    }

    public RideData? GetRideData()
    {
        return DofusApi.Datacenter.RidesData.GetRideDataById(RideId);
    }

    public RideAbilityData? GetRideAbilityData()
    {
        return DofusApi.Datacenter.RidesData.GetRideAbilityDataById(RideAbilityId);
    }

    public Description GetDescription()
    {
        var value = DofusApi.Datacenter.RidesData.GetRideNameById(RideId);

        var rideAbility = GetRideAbilityData();
        if (rideAbility is not null)
        {
            value += $" {rideAbility.Name}";
        }

        if (Infertile)
        {
            value += $" {Resources.Infertile}";
        }

        return GetDescription(value);
    }
}
