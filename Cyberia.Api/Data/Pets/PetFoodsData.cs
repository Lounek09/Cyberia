using Cyberia.Api.Data.Items;
using Cyberia.Api.Data.Monsters;
using Cyberia.Api.Factories.Effects;
using Cyberia.Api.JsonConverters;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Pets;

public class PetFoodsData
    : IDofusData
{
    [JsonPropertyName("e")]
    [JsonConverter(typeof(EffectConverter))]
    public IEffect? Effect { get; init; }

    [JsonPropertyName("i")]
    public IReadOnlyList<int> ItemsId { get; init; }

    [JsonPropertyName("it")]
    public IReadOnlyList<int> ItemTypesId { get; init; }

    [JsonPropertyName("m")]
    [JsonConverter(typeof(ReadOnlyDictionaryFromArrayConverter<int, int>))]
    public IReadOnlyDictionary<int, int> MonstersIdQuantities { get; init; }

    [JsonConstructor]
    internal PetFoodsData()
    {
        ItemsId = [];
        ItemTypesId = [];
        MonstersIdQuantities = new Dictionary<int, int>();
    }

    public IEnumerable<ItemData> GetItemsData()
    {
        foreach (var itemId in ItemTypesId)
        {
            var itemData = DofusApi.Datacenter.ItemsData.GetItemDataById(itemId);
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
            var itemTypeData = DofusApi.Datacenter.ItemsData.GetItemTypeDataById(itemTypeId);
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
            var monsterData = DofusApi.Datacenter.MonstersData.GetMonsterDataById(pair.Key);
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
            var monsterData = DofusApi.Datacenter.MonstersData.GetMonsterDataById(pair.Key);
            if (monsterData is not null)
            {
                MonstersDataQuantities.Add(monsterData, pair.Value);
            }
        }

        return MonstersDataQuantities;
    }
}
