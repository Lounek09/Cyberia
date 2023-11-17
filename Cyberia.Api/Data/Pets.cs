using Cyberia.Api.Factories.Effects;
using Cyberia.Api.JsonConverters;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public class PetFoodsData
    {
        [JsonPropertyName("e")]
        [JsonConverter(typeof(ItemEffectConverter))]
        public IEffect? Effect { get; init; }

        [JsonPropertyName("i")]
        public List<int> ItemsId { get; init; }

        [JsonPropertyName("it")]
        public List<int> ItemTypesId { get; init; }

        [JsonPropertyName("m")]
        [JsonConverter(typeof(DictionaryConverter<int, int>))]
        public Dictionary<int, int> MonstersIdQuantities { get; init; }

        [JsonConstructor]
        internal PetFoodsData()
        {
            ItemsId = [];
            ItemTypesId = [];
            MonstersIdQuantities = [];
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

        public Dictionary<MonsterData, int> GetMonstersDataQuantities()
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

            return MonstersDataQuantities;
        }
    }

    public class PetData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("tmin")]
        public TimeSpan? MinFoodInterval { get; init; }

        [JsonPropertyName("tmax")]
        public TimeSpan? MaxFoodInterval { get; init; }

        [JsonPropertyName("f")]
        public List<PetFoodsData> Foods { get; init; }

        [JsonConstructor]
        internal PetData()
        {
            Foods = [];
        }

        public ItemData? GetItemData()
        {
            return DofusApi.Datacenter.ItemsData.GetItemDataById(Id);
        }
    }

    public class PetsData
    {
        private const string FILE_NAME = "pets.json";

        [JsonPropertyName("PET")]
        public List<PetData> Pets { get; init; }

        [JsonConstructor]
        internal PetsData()
        {
            Pets = [];
        }

        internal static PetsData Load()
        {
            return Datacenter.LoadDataFromFile<PetsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public PetData? GetPetDataByItemId(int id)
        {
            return Pets.Find(x => x.Id == id);
        }
    }
}
