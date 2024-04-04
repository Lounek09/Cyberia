using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Rides;

public sealed class RidesData
    : IDofusData
{
    private const string c_fileName = "rides.json";

    private static readonly string s_filePath = Path.Join(DofusApi.OutputPath, c_fileName);

    [JsonPropertyName("RI")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, RideData>))]
    public FrozenDictionary<int, RideData> Rides { get; init; }

    [JsonPropertyName("RIA")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, RideAbilityData>))]
    public FrozenDictionary<int, RideAbilityData> RideAbilities { get; init; }

    [JsonConstructor]
    internal RidesData()
    {
        Rides = FrozenDictionary<int, RideData>.Empty;
        RideAbilities = FrozenDictionary<int, RideAbilityData>.Empty;
    }

    internal static async Task<RidesData> LoadAsync()
    {
        return await Datacenter.LoadDataAsync<RidesData>(s_filePath);
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
            ? PatternDecoder.Description(Resources.Unknown_Data, id)
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

        return rideAbilityData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : rideAbilityData.Name;
    }
}
