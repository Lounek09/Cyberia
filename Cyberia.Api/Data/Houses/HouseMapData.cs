using Cyberia.Api.Data.Maps;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Houses;

public sealed class HouseMapData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public int HouseId { get; init; }

    [JsonConstructor]
    internal HouseMapData()
    {

    }

    public MapData? GetMapData()
    {
        return DofusApi.Datacenter.MapsData.GetMapDataById(Id);
    }

    public HouseData? GetHouseData()
    {
        return DofusApi.Datacenter.HousesData.GetHouseDataById(HouseId);
    }
}
