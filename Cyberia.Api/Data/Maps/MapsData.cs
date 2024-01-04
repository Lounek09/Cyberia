using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Maps;

public sealed class MapsData
    : IDofusData
{
    private const string FILE_NAME = "maps.json";

    [JsonPropertyName("MA.m")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, MapData>))]
    public FrozenDictionary<int, MapData> Maps { get; init; }

    [JsonPropertyName("MA.sua")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, MapSuperAreaData>))]
    public FrozenDictionary<int, MapSuperAreaData> MapSuperAreas { get; init; }

    [JsonPropertyName("MA.a")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, MapAreaData>))]
    public FrozenDictionary<int, MapAreaData> MapAreas { get; init; }

    [JsonPropertyName("MA.sa")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, MapSubAreaData>))]
    public FrozenDictionary<int, MapSubAreaData> MapSubAreas { get; init; }

    [JsonConstructor]
    internal MapsData()
    {
        Maps = FrozenDictionary<int, MapData>.Empty;
        MapSuperAreas = FrozenDictionary<int, MapSuperAreaData>.Empty;
        MapAreas = FrozenDictionary<int, MapAreaData>.Empty;
        MapSubAreas = FrozenDictionary<int, MapSubAreaData>.Empty;
    }

    internal static MapsData Load()
    {
        return Datacenter.LoadDataFromFile<MapsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
    }

    public MapData? GetMapDataById(int id)
    {
        Maps.TryGetValue(id, out var mapData);
        return mapData;
    }

    public IEnumerable<MapData> GetMapsDataByCoordinate(int xCoord, int yCoord)
    {
        return Maps.Values.Where(x => x.XCoord == xCoord && x.YCoord == yCoord);
    }

    public IEnumerable<MapData> GetMapsDataByMapAreaId(int id)
    {
        return Maps.Values.Where(x => x.GetMapSubAreaData()?.GetMapAreaData()?.Id == id);
    }

    public IEnumerable<MapData> GetMapsDataByMapSubAreaId(int id)
    {
        return Maps.Values.Where(x => x.GetMapSubAreaData()?.Id == id);
    }

    public MapSuperAreaData? GetMapSuperAreaDataById(int id)
    {
        MapSuperAreas.TryGetValue(id, out var mapSuperAreaData);
        return mapSuperAreaData;
    }

    public string GetMapSuperAreaNameById(int id)
    {
        var mapSuperAreaData = GetMapSuperAreaDataById(id);

        return mapSuperAreaData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : mapSuperAreaData.Name;
    }

    public MapAreaData? GetMapAreaDataById(int id)
    {
        MapAreas.TryGetValue(id, out var mapAreaData);
        return mapAreaData;
    }

    public IEnumerable<MapAreaData> GetMapAreasDataByName(string name)
    {
        var names = name.NormalizeCustom().Split(' ');
        return MapAreas.Values.Where(x => names.All(x.Name.NormalizeCustom().Contains));
    }

    public string GetMapAreaNameById(int id)
    {
        var mapAreaData = GetMapAreaDataById(id);

        return mapAreaData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : mapAreaData.Name;
    }

    public MapSubAreaData? GetMapSubAreaDataById(int id)
    {
        MapSubAreas.TryGetValue(id, out var mapSubAreaData);
        return mapSubAreaData;
    }

    public IEnumerable<MapSubAreaData> GetMapSubAreasDataByName(string name)
    {
        var names = name.NormalizeCustom().Split(' ');
        return MapSubAreas.Values.Where(x => names.All(x.Name.NormalizeCustom().Contains));
    }

    public string GetMapSubAreaNameById(int id)
    {
        var mapSubAreaData = GetMapSubAreaDataById(id);

        return mapSubAreaData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : mapSubAreaData.Name.TrimStart("//");
    }
}
