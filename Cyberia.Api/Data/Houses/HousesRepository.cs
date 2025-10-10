using Cyberia.Api.Data.Houses.Custom;
using Cyberia.Api.Data.Houses.Localized;
using Cyberia.Api.Data.Skills;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Houses;

public sealed class HousesRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "houses.json";

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
        HousesIndoorSkillsId = ReadOnlyCollection<int>.Empty;
    }

    public HouseData? GetHouseDataById(int id)
    {
        Houses.TryGetValue(id, out var houseData);
        return houseData;
    }

    public IEnumerable<HouseData> GetHousesDataByName(string name, Language language)
    {
        return GetHousesDataByName(name, language.ToCulture());
    }

    public IEnumerable<HouseData> GetHousesDataByName(string name, CultureInfo? culture = null)
    {
        var names = name.NormalizeToAscii().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return Houses.Values.Where(x =>
        {
            return names.All(y =>
            {
                return x.Name.ToString(culture).NormalizeToAscii().Contains(y, StringComparison.OrdinalIgnoreCase);
            });
        });
    }

    public IEnumerable<HouseData> GetHousesDataByCoordinate(int x, int y)
    {
        foreach (var houseData in Houses.Values)
        {
            var map = houseData.GetOutdoorMapData();
            if (map is not null && map.X == x && map.Y == y)
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

    protected override void LoadCustomData()
    {
        var customRepository = DofusCustomRepository.Load<HousesCustomRepository>();

        foreach (var houseCustomData in customRepository.HousesCustom)
        {
            var houseData = GetHouseDataById(houseCustomData.Id);
            if (houseData is not null)
            {
                houseData.OutdoorMapId = houseCustomData.OutdoorMapId;
                houseData.RoomNumber = houseCustomData.RoomNumber;
                houseData.ChestNumber = houseCustomData.ChestNumber;
                houseData.Price = houseCustomData.Price;
            }
        }
    }

    protected override void LoadLocalizedData(LangsIdentifier identifier)
    {
        var twoLetterISOLanguageName = identifier.Language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<HousesLocalizedRepository>(identifier);

        foreach (var houseLocalizedData in localizedRepository.Houses)
        {
            var houseData = GetHouseDataById(houseLocalizedData.Id);
            if (houseData is not null)
            {
                houseData.Name.TryAdd(twoLetterISOLanguageName, houseLocalizedData.Name);
                houseData.Description.TryAdd(twoLetterISOLanguageName, houseLocalizedData.Description);
            }
        }
    }
}
