using Cyberia.Langzilla.Primitives;

using System.Globalization;
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

    public string GetMapSuperAreaName(Language language)
    {
        return GetMapSuperAreaName(language.ToCulture());
    }

    public string GetMapSuperAreaName(CultureInfo? culture = null)
    {
        return DofusApi.Datacenter.MapsRepository.GetMapSuperAreaNameById(MapSuperAreaId, culture);
    }

    public IEnumerable<MapData> GetMapsData()
    {
        return DofusApi.Datacenter.MapsRepository.GetMapsDataByMapAreaId(Id);
    }
}
