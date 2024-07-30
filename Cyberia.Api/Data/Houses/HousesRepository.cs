using Cyberia.Api.Data.Houses.Custom;
using Cyberia.Api.Data.Skills;
using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Houses;

public sealed class HousesRepository : IDofusRepository
{
    private const string c_fileName = "houses.json";

    [JsonPropertyName("H.h")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, HouseData>))]
    public FrozenDictionary<int, HouseData> Houses { get; init; }

    [JsonPropertyName("H.m")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, HouseMapData>))]
    public FrozenDictionary<int, HouseMapData> HouseMaps { get; init; }

    [JsonPropertyName("H.ids")]
    public IReadOnlyList<int> HousesIndoorSkillsId { get; init; }

    [JsonConstructor]
    internal HousesRepository()
    {
        Houses = FrozenDictionary<int, HouseData>.Empty;
        HouseMaps = FrozenDictionary<int, HouseMapData>.Empty;
        HousesIndoorSkillsId = [];
    }

    internal static HousesRepository Load(string directoryPath)
    {
        var filePath = Path.Join(directoryPath, c_fileName);
        var customFilePath = Path.Join(DofusApi.CustomPath, c_fileName);

        var data = Datacenter.LoadRepository<HousesRepository>(filePath);
        var customData = Datacenter.LoadRepository<HousesCustomRepository>(customFilePath);

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
        var names = name.NormalizeToAscii().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return Houses.Values.Where(x =>
        {
            return names.All(y =>
            {
                return ExtendString.NormalizeToAscii(x.Name).Contains(y, StringComparison.OrdinalIgnoreCase);
            });
        })
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
            var skillData = DofusApi.Datacenter.SkillsRepository.GetSkillDataById(id);
            if (skillData is not null)
            {
                yield return skillData;
            }
        }
    }
}
