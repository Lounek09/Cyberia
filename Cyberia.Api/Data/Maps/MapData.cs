using Cyberia.Api.Data.Houses;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Maps;

public sealed class MapData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("x")]
    public int XCoord { get; init; }

    [JsonPropertyName("y")]
    public int YCoord { get; init; }

    [JsonPropertyName("sa")]
    public int MapSubAreaId { get; init; }

    [JsonPropertyName("p1")]
    public string Placement1 { get; init; }

    [JsonPropertyName("p2")]
    public string Placement2 { get; init; }

    [JsonPropertyName("p")]
    public List<List<object>> Parameters { get; init; }

    [JsonPropertyName("d")]
    public int DungeonId { get; init; }

    [JsonPropertyName("c")]
    public int MaxPlayerPerFight { get; init; }

    [JsonPropertyName("t")]
    public int MaxPlayerPerTeam { get; init; }

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
        return $"[{XCoord}, {YCoord}]";
    }

    public string GetImagePath()
    {
        return $"{DofusApi.Config.CdnUrl}/images/dofus/maps/{Id}.jpg";
    }

    public MapSubAreaData? GetMapSubAreaData()
    {
        return DofusApi.Datacenter.MapsData.GetMapSubAreaDataById(MapSubAreaId);
    }

    public string GetMapAreaName()
    {
        var mapSubAreaData = GetMapSubAreaData();
        var mapSubAreaName = mapSubAreaData is null ? $"{nameof(MapSubAreaData)} {PatternDecoder.Description(Resources.Unknown_Data, MapSubAreaId)}" : mapSubAreaData.Name.TrimStart("//");

        var mapAreaData = mapSubAreaData?.GetMapAreaData();
        var mapAreaName = mapAreaData is null ? $"{nameof(MapAreaData)} {PatternDecoder.Description(Resources.Unknown_Data, mapSubAreaData?.MapAreaId ?? 0)}" : mapAreaData.Name;

        return mapAreaName + (mapAreaName.Equals(mapSubAreaName) ? string.Empty : $" ({mapSubAreaName})");
    }

    public HouseData? GetHouseData()
    {
        var houseMapData = DofusApi.Datacenter.HousesData.GetHouseMapDataById(Id);

        return houseMapData?.GetHouseData();
    }

    public bool IsHouse()
    {
        return GetHouseData() is not null;
    }
}
