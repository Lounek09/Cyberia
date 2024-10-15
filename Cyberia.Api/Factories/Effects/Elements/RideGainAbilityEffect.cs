using Cyberia.Api.Data.Rides;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record RideGainAbilityEffect : Effect
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
        return DofusApi.Datacenter.RidesRepository.GetRideAbilityDataById(RideAbilityId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var rideAbilityName = DofusApi.Datacenter.RidesRepository.GetRideAbilityNameById(RideAbilityId, culture);

        return GetDescription(culture, string.Empty, string.Empty, rideAbilityName);
    }
}
