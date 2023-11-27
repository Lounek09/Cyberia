using Cyberia.Api.Data.Custom;
using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data;

public sealed class HouseData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("d")]
    public string Description { get; init; }

    [JsonIgnore]
    public int OutdoorMapId { get; internal set; }

    [JsonIgnore]
    public int RoomNumber { get; internal set; }

    [JsonIgnore]
    public int ChestNumber { get; internal set; }

    [JsonIgnore]
    public int Price { get; internal set; }

    [JsonConstructor]
    internal HouseData()
    {
        Name = string.Empty;
        Description = string.Empty;
    }

    public IEnumerable<MapData> GetMapsData()
    {
        foreach (var houseMapData in DofusApi.Datacenter.HousesData.GetHouseMapsDataByHouseId(Id))
        {
            var mapData = houseMapData.GetMapData();
            if (mapData is not null)
            {
                yield return mapData;
            }
        }
    }

    public MapData? GetOutdoorMapData()
    {
        return DofusApi.Datacenter.MapsData.GetMapDataById(OutdoorMapId);
    }

    public string GetCoordinate()
    {
        var mapData = GetOutdoorMapData();

        return mapData is null ? "[x, x]" : mapData.GetCoordinate();
    }
}

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

public sealed class HousesData : IDofusData
{
    private const string FILE_NAME = "houses.json";

    [JsonPropertyName("H.h")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, HouseData>))]
    public FrozenDictionary<int, HouseData> Houses { get; init; }

    [JsonPropertyName("H.m")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, HouseMapData>))]
    public FrozenDictionary<int, HouseMapData> HouseMaps { get; init; }

    [JsonPropertyName("H.ids")]
    [JsonConverter(typeof(ReadOnlyCollectionConverter<int>))]
    public ReadOnlyCollection<int> HousesIndoorSkillsId { get; init; }

    [JsonConstructor]
    internal HousesData()
    {
        Houses = FrozenDictionary<int, HouseData>.Empty;
        HouseMaps = FrozenDictionary<int, HouseMapData>.Empty;
        HousesIndoorSkillsId = ReadOnlyCollection<int>.Empty;
    }

    internal static HousesData Load()
    {
        var data = Datacenter.LoadDataFromFile<HousesData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        var customData = Datacenter.LoadDataFromFile<HousesCustomData>(Path.Combine(DofusApi.CUSTOM_PATH, FILE_NAME));

        foreach (var houseCustomData in customData.HousesCustom)
        {
            var houseData = data.GetHouseDataById(houseCustomData.Id);
            if (houseData is not null)
            {
                houseData.OutdoorMapId = houseCustomData.OutdoorMapId;
                houseData.RoomNumber = houseCustomData.RoomNumber;
                houseData.ChestNumber = houseCustomData.ChestNumber;
                houseData.Price = houseCustomData.Price;
            }
        }

        return data;
    }

    public HouseData? GetHouseDataById(int id)
    {
        Houses.TryGetValue(id, out var houseData);
        return houseData;
    }

    public IEnumerable<HouseData> GetHousesDataByName(string name)
    {
        var names = name.NormalizeCustom().Split(' ');
        return Houses.Values.Where(x => names.All(x.Name.NormalizeCustom().Contains))
            .OrderBy(x => x.Id);
    }

    public IEnumerable<HouseData> GetHousesDataByCoordinate(int x, int y)
    {
        foreach (var houseData in Houses.Values)
        {
            var map = houseData.GetOutdoorMapData();
            if (map is not null && map.XCoord == x && map.YCoord == y)
            {
                yield return houseData;
            }
        }
    }

    public IEnumerable<HouseData> GetHousesDataByMapSubAreaId(int id)
    {
        foreach (var houseData in Houses.Values)
        {
            var map = houseData.GetOutdoorMapData();
            if (map is not null && map.GetMapSubAreaData()?.Id == id)
            {
                yield return houseData;
            }
        }
    }

    public IEnumerable<HouseData> GetHousesDataByMapAreaId(int id)
    {
        foreach (var houseData in Houses.Values)
        {
            var map = houseData.GetOutdoorMapData();
            if (map is not null && map.GetMapSubAreaData()?.GetMapAreaData()?.Id == id)
            {
                yield return houseData;
            }
        }
    }

    internal HouseMapData? GetHouseMapDataById(int id)
    {
        HouseMaps.TryGetValue(id, out var houseMapData);
        return houseMapData;
    }

    internal IEnumerable<HouseMapData> GetHouseMapsDataByHouseId(int id)
    {
        return HouseMaps.Values.Where(x => x.HouseId == id);
    }

    public IEnumerable<SkillData> GetHousesIndoorsSkill()
    {
        foreach (var id in HousesIndoorSkillsId)
        {
            var skillData = DofusApi.Datacenter.SkillsData.GetSkillDataById(id);
            if (skillData is not null)
            {
                yield return skillData;
            }
        }
    }
}
