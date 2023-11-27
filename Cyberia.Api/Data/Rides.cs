using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data;

public sealed class RideData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("g")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int GfxId { get; init; }

    [JsonPropertyName("c1")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int Color1 { get; init; }

    [JsonPropertyName("c2")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int Color2 { get; init; }

    [JsonPropertyName("c3")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int Color3 { get; init; }

    [JsonConstructor]
    internal RideData()
    {
        Name = string.Empty;
    }
}

public sealed class RideAbilityData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("d")]
    public string Description { get; init; }

    [JsonPropertyName("e")]
    [JsonInclude]
    internal string E { get; init; }

    [JsonConstructor]
    internal RideAbilityData()
    {
        Name = string.Empty;
        Description = string.Empty;
        E = string.Empty;
    }
}

public sealed class RidesData : IDofusData
{
    private const string FILE_NAME = "rides.json";

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

    internal static RidesData Load()
    {
        return Datacenter.LoadDataFromFile<RidesData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
    }

    public RideData? GetRideDataById(int id)
    {
        Rides.TryGetValue(id, out var rideData);
        return rideData;
    }

    public string GetRideNameById(int id)
    {
        var rideData = GetRideDataById(id);

        return rideData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : rideData.Name;
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
