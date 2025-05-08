using Cyberia.Api.Data.Houses;
using Cyberia.Api.Managers;
using Cyberia.Langzilla.Enums;

using System.Globalization;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Maps;

public sealed class MapData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("x")]
    public int X { get; init; }

    [JsonPropertyName("y")]
    public int Y { get; init; }

    [JsonPropertyName("sa")]
    public int MapSubAreaId { get; init; }

    [JsonPropertyName("p1")]
    public string Placement1 { get; init; }

    [JsonPropertyName("p2")]
    public string Placement2 { get; init; }

    [JsonPropertyName("p")]
    [JsonInclude]
    internal List<List<object>> Parameters { get; init; }

    [JsonPropertyName("d")]
    public int DungeonId { get; init; }

    [JsonPropertyName("c")]
    public int MaxPlayerPerFight { get; init; }

    [JsonPropertyName("t")]
    public int MaxPlayerPerTeam { get; init; }

    [JsonPropertyName("tournament")]
    public bool Tournament { get; init; }

    [JsonPropertyName("ep")]
    public int Episode { get; init; }

    [JsonConstructor]
    internal MapData()
    {
        Placement1 = string.Empty;
        Placement2 = string.Empty;
        Parameters = [];
        MaxPlayerPerFight = 16;
        MaxPlayerPerTeam = 8;
    }

    public string GetCoordinate()
    {
        return $"[{X}, {Y}]";
    }

    public async Task<string> GetImagePathAsync()
    {
        return await CdnManager.GetImagePathAsync("maps", Id, "jpg");
    }

    public IEnumerable<MapData> GetMapsDataAtSameCoordinate()
    {
        return DofusApi.Datacenter.MapsRepository.GetMapsDataByCoordinate(X, Y);
    }

    public MapSubAreaData? GetMapSubAreaData()
    {
        return DofusApi.Datacenter.MapsRepository.GetMapSubAreaDataById(MapSubAreaId);
    }

    public string GetMapAreaName(Language language)
    {
        return GetMapAreaName(language.ToCulture());
    }

    public string GetMapAreaName(CultureInfo? culture = null)
    {
        var mapSubAreaData = GetMapSubAreaData();
        var mapSubAreaName = mapSubAreaData is null
            ? $"{nameof(MapSubAreaData)} {Translation.UnknownData(MapSubAreaId, culture)}"
            : mapSubAreaData.Name.ToString(culture).TrimStart('/');

        var mapAreaData = mapSubAreaData?.GetMapAreaData();
        var mapAreaName = mapAreaData is null
            ? $"{nameof(MapAreaData)} {Translation.UnknownData(mapSubAreaData?.MapAreaId ?? 0, culture)}"
            : mapAreaData.Name.ToString(culture);

        return mapAreaName + (mapAreaName.Equals(mapSubAreaName) ? string.Empty : $" ({mapSubAreaName})");
    }

    public HouseData? GetHouseData()
    {
        var houseMapData = DofusApi.Datacenter.HousesRepository.GetHouseMapDataById(Id);

        return houseMapData?.GetHouseData();
    }

    public bool IsHouse()
    {
        return GetHouseData() is not null;
    }
}
