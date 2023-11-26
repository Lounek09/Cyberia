using Cyberia.Api.Factories.Effects;
using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public class PetFoodsData : IDofusData
    {
        [JsonPropertyName("e")]
        [JsonConverter(typeof(ItemEffectConverter))]
        public IEffect? Effect { get; init; }

        [JsonPropertyName("i")]
        [JsonConverter(typeof(ReadOnlyCollectionConverter<int>))]
        public ReadOnlyCollection<int> ItemsId { get; init; }

        [JsonPropertyName("it")]
        [JsonConverter(typeof(ReadOnlyCollectionConverter<int>))]
        public ReadOnlyCollection<int> ItemTypesId { get; init; }

        [JsonPropertyName("m")]
        [JsonConverter(typeof(ReadOnlyDictionaryFromArrayConverter<int, int>))]
        public ReadOnlyDictionary<int, int> MonstersIdQuantities { get; init; }

        [JsonConstructor]
        internal PetFoodsData()
        {
            ItemsId = ReadOnlyCollection<int>.Empty;
            ItemTypesId = ReadOnlyCollection<int>.Empty;
            MonstersIdQuantities = ReadOnlyDictionary<int, int>.Empty;
        }

        public IEnumerable<ItemData> GetItemsData()
        {
            foreach (int itemId in ItemTypesId)
            {
                ItemData? itemData = DofusApi.Datacenter.ItemsData.GetItemDataById(itemId);
                if (itemData is not null)
                {
                    yield return itemData;
                }
            }
        }

        public IEnumerable<ItemTypeData> GetItemTypesData()
        {
            foreach (int itemTypeId in ItemTypesId)
            {
                ItemTypeData? itemTypeData = DofusApi.Datacenter.ItemsData.GetItemTypeDataById(itemTypeId);
                if (itemTypeData is not null)
                {
                    yield return itemTypeData;
                }
            }
        }

        public IEnumerable<MonsterData> GetMonstersData()
        {
            foreach (KeyValuePair<int, int> pair in MonstersIdQuantities)
            {
                MonsterData? monsterData = DofusApi.Datacenter.MonstersData.GetMonsterDataById(pair.Key);
                if (monsterData is not null)
                {
                    yield return monsterData;
                }
            }
        }

        public ReadOnlyDictionary<MonsterData, int> GetMonstersDataQuantities()
        {
            Dictionary<MonsterData, int> MonstersDataQuantities = [];

            foreach (KeyValuePair<int, int> pair in MonstersIdQuantities)
            {
                MonsterData? monsterData = DofusApi.Datacenter.MonstersData.GetMonsterDataById(pair.Key);
                if (monsterData is not null)
                {
                    MonstersDataQuantities.Add(monsterData, pair.Value);
                }
            }

            return MonstersDataQuantities.AsReadOnly();
        }
    }

    public class PetData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("tmin")]
        public TimeSpan? MinFoodInterval { get; init; }

        [JsonPropertyName("tmax")]
        public TimeSpan? MaxFoodInterval { get; init; }

        [JsonPropertyName("f")]
        [JsonConverter(typeof(ReadOnlyCollectionConverter<PetFoodsData>))]
        public ReadOnlyCollection<PetFoodsData> Foods { get; init; }

        [JsonConstructor]
        internal PetData()
        {
            Foods = ReadOnlyCollection<PetFoodsData>.Empty;
        }

        public ItemData? GetItemData()
        {
            return DofusApi.Datacenter.ItemsData.GetItemDataById(Id);
        }
    }

    public class PetsData : IDofusData
    {
        private const string FILE_NAME = "pets.json";

        [JsonPropertyName("PET")]
        [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, PetData>))]
        public FrozenDictionary<int, PetData> Pets { get; init; }

        [JsonConstructor]
        internal PetsData()
        {
            Pets = FrozenDictionary<int, PetData>.Empty;
        }

        internal static PetsData Load()
        {
            return Datacenter.LoadDataFromFile<PetsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public PetData? GetPetDataByItemId(int id)
        {
            Pets.TryGetValue(id, out PetData? petData);
            return petData;
        }
    }
}
