using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Maps;

public sealed class MapAreaData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public LocalizedString Name { get; init; }

    [JsonPropertyName("sua")]
    public int MapSuperAreaId { get; init; }

    [JsonConstructor]
    internal MapAreaData()
    {
        Name = LocalizedString.Empty;
    }

    public MapSuperAreaData? GetMapSuperAreaData()
    {
        return DofusApi.Datacenter.MapsRepository.GetMapSuperAreaDataById(MapSuperAreaId);
    }

    public IEnumerable<MapData> GetMapsData()
    {
        return DofusApi.Datacenter.MapsRepository.GetMapsDataByMapAreaId(Id);
    }
}
