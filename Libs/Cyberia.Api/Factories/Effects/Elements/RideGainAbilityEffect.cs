using Cyberia.Api.Data.Rides;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Interfaces;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record RideGainAbilityEffect : Effect, IRideAbilityEffect
{
    public int RideAbilityId { get; init; }

    private RideGainAbilityEffect(int id, int rideAbilityId)
        : base(id)
    {
        RideAbilityId = rideAbilityId;
    }

    internal static RideGainAbilityEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public RideAbilityData? GetRideAbilityData()
    {
        return DofusApi.Datacenter.RidesRepository.GetRideAbilityDataById(RideAbilityId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var rideAbilityName = DofusApi.Datacenter.RidesRepository.GetRideAbilityNameById(RideAbilityId, culture);

        return GetDescription(culture, string.Empty, string.Empty, rideAbilityName);
    }
}
