using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Rides;

public sealed class RidesRepository : IDofusRepository
{
    private const string c_fileName = "rides.json";

    [JsonPropertyName("RI")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, RideData>))]
    public FrozenDictionary<int, RideData> Rides { get; init; }

    [JsonPropertyName("RIA")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, RideAbilityData>))]
    public FrozenDictionary<int, RideAbilityData> RideAbilities { get; init; }

    [JsonConstructor]
    internal RidesRepository()
    {
        Rides = FrozenDictionary<int, RideData>.Empty;
        RideAbilities = FrozenDictionary<int, RideAbilityData>.Empty;
    }

    internal static RidesRepository Load(string directoryPath)
    {
        var filePath = Path.Join(directoryPath, c_fileName);

        return Datacenter.LoadRepository<RidesRepository>(filePath);
    }

    public RideData? GetRideDataById(int id)
    {
        Rides.TryGetValue(id, out var rideData);
        return rideData;
    }

    public string GetRideNameById(int id)
    {
        var rideData = GetRideDataById(id);

        return rideData is null
            ? Translation.Format(ApiTranslations.Unknown_Data, id)
            : rideData.Name;
    }

    public RideAbilityData? GetRideAbilityDataById(int id)
    {
        RideAbilities.TryGetValue(id, out var rideAbilityData);
        return rideAbilityData;
    }

    public string GetRideAbilityNameById(int id)
    {
        var rideAbilityData = GetRideAbilityDataById(id);

        return rideAbilityData is null ? Translation.Format(ApiTranslations.Unknown_Data, id) : rideAbilityData.Name;
    }
}
