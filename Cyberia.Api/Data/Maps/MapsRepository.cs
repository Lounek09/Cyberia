using Cyberia.Api.Data.Maps.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
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

    public string GetMapSuperAreaNameById(int id)
    {
        var mapSuperAreaData = GetMapSuperAreaDataById(id);

        return mapSuperAreaData is null
            ? Translation.Format(ApiTranslations.Unknown_Data, id)
            : mapSuperAreaData.Name;
    }

    public MapAreaData? GetMapAreaDataById(int id)
    {
        MapAreas.TryGetValue(id, out var mapAreaData);
        return mapAreaData;
    }

    public IEnumerable<MapAreaData> GetMapAreasDataByName(string name)
    {
        var names = name.NormalizeToAscii().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return MapAreas.Values.Where(x =>
        {
            return names.All(y =>
            {
                return StringExtensions.NormalizeToAscii(x.Name).Contains(y, StringComparison.OrdinalIgnoreCase);
            });
        });
    }

    public string GetMapAreaNameById(int id)
    {
        var mapAreaData = GetMapAreaDataById(id);

        return mapAreaData is null
            ? Translation.Format(ApiTranslations.Unknown_Data, id)
            : mapAreaData.Name;
    }

    public MapSubAreaData? GetMapSubAreaDataById(int id)
    {
        MapSubAreas.TryGetValue(id, out var mapSubAreaData);
        return mapSubAreaData;
    }

    public IEnumerable<MapSubAreaData> GetMapSubAreasDataByName(string name)
    {
        var names = name.NormalizeToAscii().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return MapSubAreas.Values.Where(x =>
        {
            return names.All(y =>
            {
                return StringExtensions.NormalizeToAscii(x.Name).Contains(y, StringComparison.OrdinalIgnoreCase);
            });
        });
    }

    public string GetMapSubAreaNameById(int id)
    {
        var mapSubAreaData = GetMapSubAreaDataById(id);

        return mapSubAreaData is null
            ? Translation.Format(ApiTranslations.Unknown_Data, id)
            : mapSubAreaData.Name.ToString().TrimStart('/');
    }

    protected override void LoadLocalizedData(LangType type, LangLanguage language)
    {
        var twoLetterISOLanguageName = language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<MapsLocalizedRepository>(type, language);

        foreach (var mapSuperAreaLocalizedData in localizedRepository.MapSuperAreas)
        {
            var mapSuperAreaData = GetMapSuperAreaDataById(mapSuperAreaLocalizedData.Id);
            mapSuperAreaData?.Name.Add(twoLetterISOLanguageName, mapSuperAreaLocalizedData.Name);
        }

        foreach (var mapAreaLocalizedData in localizedRepository.MapAreas)
        {
            var mapAreaData = GetMapAreaDataById(mapAreaLocalizedData.Id);
            mapAreaData?.Name.Add(twoLetterISOLanguageName, mapAreaLocalizedData.Name);
        }

        foreach (var mapSubAreaLocalizedData in localizedRepository.MapSubAreas)
        {
            var mapSubAreaData = GetMapSubAreaDataById(mapSubAreaLocalizedData.Id);
            mapSubAreaData?.Name.Add(twoLetterISOLanguageName, mapSubAreaLocalizedData.Name);
        }
    }
}
