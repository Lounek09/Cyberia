using Cyberia.Api.Data.Items;
using Cyberia.Api.Data.Monsters;
using Cyberia.Api.Factories.Effects;
using Cyberia.Api.JsonConverters;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Pets;

public sealed class PetFoodsData : IDofusData
{
    [JsonPropertyName("e")]
    [JsonConverter(typeof(EffectConverter))]
    public IEffect Effect { get; init; }

    [JsonPropertyName("i")]
    public IReadOnlyList<int> ItemsId { get; init; }

    [JsonPropertyName("it")]
    public IReadOnlyList<int> ItemTypesId { get; init; }

    [JsonPropertyName("m")]
    [JsonConverter(typeof(ReadOnlyDictionaryFromArrayConverter<int, int>))]
    public IReadOnlyDictionary<int, int> MonstersIdQuantities { get; init; }

    [JsonPropertyName("mr")]
    [JsonConverter(typeof(ReadOnlyDictionaryFromArrayConverter<int, int>))]
    public IReadOnlyDictionary<int, int> MonsterRacesIdQuantities { get; init; }

    [JsonPropertyName("msr")]
    [JsonConverter(typeof(ReadOnlyDictionaryFromArrayConverter<int, int>))]
    public IReadOnlyDictionary<int, int> MonsterSuperRacesIdQuantities { get; init; }

    [JsonConstructor]
    internal PetFoodsData()
    {
        Effect = new ErroredEffect(string.Empty);
        ItemsId = [];
        ItemTypesId = [];
        MonstersIdQuantities = new Dictionary<int, int>();
        MonsterRacesIdQuantities = new Dictionary<int, int>();
        MonsterSuperRacesIdQuantities = new Dictionary<int, int>();
    }

    public IEnumerable<ItemData> GetItemsData()
    {
        foreach (var itemId in ItemTypesId)
        {
            var itemData = DofusApi.Datacenter.ItemsRepository.GetItemDataById(itemId);
            if (itemData is not null)
            {
                yield return itemData;
            }
        }
    }

    public IEnumerable<ItemTypeData> GetItemTypesData()
    {
        foreach (var itemTypeId in ItemTypesId)
        {
            var itemTypeData = DofusApi.Datacenter.ItemsRepository.GetItemTypeDataById(itemTypeId);
            if (itemTypeData is not null)
            {
                yield return itemTypeData;
            }
        }
    }

    public IEnumerable<MonsterData> GetMonstersData()
    {
        foreach (var pair in MonstersIdQuantities)
        {
            var monsterData = DofusApi.Datacenter.MonstersRepository.GetMonsterDataById(pair.Key);
            if (monsterData is not null)
            {
                yield return monsterData;
            }
        }
    }

    public IReadOnlyDictionary<MonsterData, int> GetMonstersDataQuantities()
    {
        Dictionary<MonsterData, int> MonstersDataQuantities = [];

        foreach (var pair in MonstersIdQuantities)
        {
            var monsterData = DofusApi.Datacenter.MonstersRepository.GetMonsterDataById(pair.Key);
            if (monsterData is not null)
            {
                MonstersDataQuantities.Add(monsterData, pair.Value);
            }
        }

        return MonstersDataQuantities;
    }

    public IEnumerable<MonsterRaceData> GetMonsterRacessData()
    {
        foreach (var pair in MonsterRacesIdQuantities)
        {
            var monsterRaceData = DofusApi.Datacenter.MonstersRepository.GetMonsterRaceDataById(pair.Key);
            if (monsterRaceData is not null)
            {
                yield return monsterRaceData;
            }
        }
    }

    public IReadOnlyDictionary<MonsterRaceData, int> GetMonsterRacesDataQuantities()
    {
        Dictionary<MonsterRaceData, int> MonsterRacesDataQuantities = [];

        foreach (var pair in MonsterRacesIdQuantities)
        {
            var monsterRaceData = DofusApi.Datacenter.MonstersRepository.GetMonsterRaceDataById(pair.Key);
            if (monsterRaceData is not null)
            {
                MonsterRacesDataQuantities.Add(monsterRaceData, pair.Value);
            }
        }

        return MonsterRacesDataQuantities;
    }

    public IEnumerable<MonsterSuperRaceData> GetMonsterSuperRacesData()
    {
        foreach (var pair in MonsterSuperRacesIdQuantities)
        {
            var monsterSuperRaceData = DofusApi.Datacenter.MonstersRepository.GetMonsterSuperRaceDataById(pair.Key);
            if (monsterSuperRaceData is not null)
            {
                yield return monsterSuperRaceData;
            }
        }
    }

    public IReadOnlyDictionary<MonsterSuperRaceData, int> GetMonsterSuperRacesDataQuantities()
    {
        Dictionary<MonsterSuperRaceData, int> MonsterSuperRacesDataQuantities = [];

        foreach (var pair in MonstersIdQuantities)
        {
            var monsterSuperRaceData = DofusApi.Datacenter.MonstersRepository.GetMonsterSuperRaceDataById(pair.Key);
            if (monsterSuperRaceData is not null)
            {
                MonsterSuperRacesDataQuantities.Add(monsterSuperRaceData, pair.Value);
            }
        }

        return MonsterSuperRacesDataQuantities;
    }
}
