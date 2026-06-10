using Cyberia.Api.Data.Rides;

namespace Cyberia.Api.Factories.Effects.Interfaces;

public interface IRideEffect
{
    int RideId { get; }

    RideData? GetRideData();
}
