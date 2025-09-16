using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Rides.Localized;

internal sealed class RidesLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => RidesRepository.FileName;

    [JsonPropertyName("RI")]
    public IReadOnlyList<RideLocalizedData> Rides { get; init; }

    [JsonPropertyName("RIA")]
    public IReadOnlyList<RideAbilityLocalizedData> RideAbilities { get; init; }

    [JsonConstructor]
    internal RidesLocalizedRepository()
    {
        Rides = ReadOnlyCollection<RideLocalizedData>.Empty;
        RideAbilities = ReadOnlyCollection<RideAbilityLocalizedData>.Empty;
    }
}
