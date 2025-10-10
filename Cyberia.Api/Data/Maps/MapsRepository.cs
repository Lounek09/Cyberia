using Cyberia.Api.Data.Maps.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Maps;

public sealed class MapsRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "maps.json";

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
    internal MapsRepository()
    {
        Maps = FrozenDictionary<int, MapData>.Empty;
        MapSuperAreas = FrozenDictionary<int, MapSuperAreaData>.Empty;
        MapAreas = FrozenDictionary<int, MapAreaData>.Empty;
        MapSubAreas = FrozenDictionary<int, MapSubAreaData>.Empty;
    }

    public MapData? GetMapDataById(int id)
    {
        Maps.TryGetValue(id, out var mapData);
        return mapData;
    }

    public IEnumerable<MapData> GetMapsDataByCoordinate(int xCoord, int yCoord)
    {
        return Maps.Values.Where(x => x.X == xCoord && x.Y == yCoord);
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

    public string GetMapSuperAreaNameById(int id, Language language)
    {
        return GetMapSuperAreaNameById(id, language.ToCulture());
    }

    public string GetMapSuperAreaNameById(int id, CultureInfo? culture = null)
    {
        var mapSuperAreaData = GetMapSuperAreaDataById(id);

        return mapSuperAreaData is null
            ? Translation.UnknownData(id, culture)
            : mapSuperAreaData.Name.ToString(culture);
    }

    public MapAreaData? GetMapAreaDataById(int id)
    {
        MapAreas.TryGetValue(id, out var mapAreaData);
        return mapAreaData;
    }

    public IEnumerable<MapAreaData> GetMapAreasDataByName(string name, Language language)
    {
        return GetMapAreasDataByName(name, language.ToCulture());
    }

    public IEnumerable<MapAreaData> GetMapAreasDataByName(string name, CultureInfo? culture = null)
    {
        var names = name.NormalizeToAscii().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return MapAreas.Values.Where(x =>
        {
            return names.All(y =>
            {
                return x.Name.ToString(culture).NormalizeToAscii().Contains(y, StringComparison.OrdinalIgnoreCase);
            });
        });
    }

    public string GetMapAreaNameById(int id, Language language)
    {
        return GetMapAreaNameById(id, language.ToCulture());
    }

    public string GetMapAreaNameById(int id, CultureInfo? culture = null)
    {
        var mapAreaData = GetMapAreaDataById(id);

        return mapAreaData is null
            ? Translation.UnknownData(id, culture)
            : mapAreaData.Name.ToString(culture);
    }

    public MapSubAreaData? GetMapSubAreaDataById(int id)
    {
        MapSubAreas.TryGetValue(id, out var mapSubAreaData);
        return mapSubAreaData;
    }

    public IEnumerable<MapSubAreaData> GetMapSubAreasDataByName(string name, Language language)
    {
        return GetMapSubAreasDataByName(name, language.ToCulture());
    }

    public IEnumerable<MapSubAreaData> GetMapSubAreasDataByName(string name, CultureInfo? culture = null)
    {
        var names = name.NormalizeToAscii().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return MapSubAreas.Values.Where(x =>
        {
            return names.All(y =>
            {
                return x.Name.ToString(culture).NormalizeToAscii().Contains(y, StringComparison.OrdinalIgnoreCase);
            });
        });
    }

    public string GetMapSubAreaNameById(int id, Language language)
    {
        return GetMapSubAreaNameById(id, language.ToCulture());
    }

    public string GetMapSubAreaNameById(int id, CultureInfo? culture = null)
    {
        var mapSubAreaData = GetMapSubAreaDataById(id);

        return mapSubAreaData is null
            ? Translation.UnknownData(id, culture)
            : mapSubAreaData.Name.ToString(culture).TrimStart('/');
    }

    protected override void LoadLocalizedData(LangsIdentifier identifier)
    {
        var twoLetterISOLanguageName = identifier.Language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<MapsLocalizedRepository>(identifier);

        foreach (var mapLocalizedData in localizedRepository.Maps)
        {
            var mapData = GetMapDataById(mapLocalizedData.Id);
            if (mapData?.Name is not null)
            {
                mapData.Name.Value.TryAdd(twoLetterISOLanguageName, mapLocalizedData.Name);
            }
        }

        foreach (var mapSuperAreaLocalizedData in localizedRepository.MapSuperAreas)
        {
            var mapSuperAreaData = GetMapSuperAreaDataById(mapSuperAreaLocalizedData.Id);
            mapSuperAreaData?.Name.TryAdd(twoLetterISOLanguageName, mapSuperAreaLocalizedData.Name);
        }

        foreach (var mapAreaLocalizedData in localizedRepository.MapAreas)
        {
            var mapAreaData = GetMapAreaDataById(mapAreaLocalizedData.Id);
            mapAreaData?.Name.TryAdd(twoLetterISOLanguageName, mapAreaLocalizedData.Name);
        }

        foreach (var mapSubAreaLocalizedData in localizedRepository.MapSubAreas)
        {
            var mapSubAreaData = GetMapSubAreaDataById(mapSubAreaLocalizedData.Id);
            mapSubAreaData?.Name.TryAdd(twoLetterISOLanguageName, mapSubAreaLocalizedData.Name.TrimStart('/'));
        }
    }
}
