using Cyberia.Api.Data.Houses;
using Cyberia.Api.JsonConverters;
using Cyberia.Api.Utils;
using Cyberia.Langzilla.Primitives;

using System.Collections.ObjectModel;
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
    [JsonConverter(typeof(MapPlacementConverter))]
    public IReadOnlyList<int> Placement1 { get; init; }

    [JsonPropertyName("p2")]
    [JsonConverter(typeof(MapPlacementConverter))]
    public IReadOnlyList<int> Placement2 { get; init; }

    [JsonPropertyName("p")]
    [JsonInclude]
    internal IReadOnlyCollection<IReadOnlyCollection<object>> Parameters { get; init; }

    [JsonPropertyName("d")]
    public int DungeonId { get; init; }

    [JsonPropertyName("n")]
    public LocalizedString? Name { get; init; }

    [JsonPropertyName("c")]
    public int MaxPlayerPerFight { get; init; }

    [JsonPropertyName("t")]
    public int MaxPlayerPerTeam { get; init; }

    [JsonPropertyName("tournament")]
    public bool Tournament { get; init; }

    [JsonPropertyName("o")]
    public bool Outdoor { get; init; }

    [JsonPropertyName("ep")]
    public int Episode { get; init; }

    [JsonConstructor]
    internal MapData()
    {
        Placement1 = ReadOnlyCollection<int>.Empty;
        Placement2 = ReadOnlyCollection<int>.Empty;
        Parameters = ReadOnlyCollection<IReadOnlyCollection<object>>.Empty;
        MaxPlayerPerFight = 16;
        MaxPlayerPerTeam = 8;
    }

    public string GetCoordinate()
    {
        return $"[{X}, {Y}]";
    }

    public async Task<string> GetImagePathAsync()
    {
        return await ImageUrlProvider.GetImagePathAsync("maps", Id, "jpg");
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
        var mapSubAreaName = mapSubAreaData?.Name.ToString(culture).TrimStart('/')
            ?? $"{nameof(MapSubAreaData)} {Translation.UnknownData(MapSubAreaId, culture)}";

        var mapAreaData = mapSubAreaData?.GetMapAreaData();
        var mapAreaName = mapAreaData?.Name.ToString(culture)
            ?? $"{nameof(MapAreaData)} {Translation.UnknownData(mapSubAreaData?.MapAreaId ?? 0, culture)}";

        return mapAreaName.Equals(mapSubAreaName, StringComparison.OrdinalIgnoreCase)
            ? mapAreaName
            : $"{mapAreaName} ({mapSubAreaName})";
    }

    public string GetFullName(Language language)
    {
        return GetFullName(language.ToCulture());
    }

    public string GetFullName(CultureInfo? culture = null)
    {
        var name = GetMapAreaName(culture);

        if (Name.HasValue)
        {
            name += $" - {Name.Value.ToString(culture)}";
        }

        return name;
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
