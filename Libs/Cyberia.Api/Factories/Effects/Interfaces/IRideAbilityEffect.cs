using Cyberia.Api.Data.Rides;

namespace Cyberia.Api.Factories.Effects.Interfaces;

public interface IRideAbilityEffect
{
    int RideAbilityId { get; }

    RideAbilityData? GetRideAbilityData();
}
