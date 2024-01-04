using Cyberia.Api.Data.Houses.Custom;
using Cyberia.Api.Data.Skills;
using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Houses;

public sealed class HousesData
    : IDofusData
{
    private const string FILE_NAME = "houses.json";

    [JsonPropertyName("H.h")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, HouseData>))]
    public FrozenDictionary<int, HouseData> Houses { get; init; }

    [JsonPropertyName("H.m")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, HouseMapData>))]
    public FrozenDictionary<int, HouseMapData> HouseMaps { get; init; }

    [JsonPropertyName("H.ids")]
    public IReadOnlyList<int> HousesIndoorSkillsId { get; init; }

    [JsonConstructor]
    internal HousesData()
    {
        Houses = FrozenDictionary<int, HouseData>.Empty;
        HouseMaps = FrozenDictionary<int, HouseMapData>.Empty;
        HousesIndoorSkillsId = [];
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
