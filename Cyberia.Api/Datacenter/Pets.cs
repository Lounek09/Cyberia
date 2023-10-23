using Cyberia.Api.Factories.Effects;
using Cyberia.Api.Parser.JsonConverter;

using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public class PetFoodsData
    {
        [JsonPropertyName("e")]
        [JsonConverter(typeof(ItemEffectJsonConverter))]
        public IEffect? Effect { get; init; }

        [JsonPropertyName("i")]
        public List<int> ItemsId { get; init; }

        [JsonPropertyName("it")]
        public List<int> ItemTypesId { get; init; }

        [JsonPropertyName("m")]
        [JsonConverter(typeof(DictionaryJsonConverter<int, int>))]
        public Dictionary<int, int> MonstersIdQuantities { get; init; }

        public PetFoodsData()
        {
            ItemsId = new();
            ItemTypesId = new();
            MonstersIdQuantities = new();
        }

        public IEnumerable<ItemData> GetItemsData()
        {
            foreach (int itemId in ItemTypesId)
            {
                ItemData? itemData = DofusApi.Instance.Datacenter.ItemsData.GetItemDataById(itemId);
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
                ItemTypeData? itemTypeData = DofusApi.Instance.Datacenter.ItemsData.GetItemTypeDataById(itemTypeId);
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
                MonsterData? monsterData = DofusApi.Instance.Datacenter.MonstersData.GetMonsterDataById(pair.Key);
                if (monsterData is not null)
                {
                    yield return monsterData;
                }
            }
        }

        public Dictionary<MonsterData, int> GetMonstersDataQuantities()
        {
            Dictionary<MonsterData, int> MonstersDataQuantities = new();

            foreach (KeyValuePair<int, int> pair in MonstersIdQuantities)
            {
                MonsterData? monsterData = DofusApi.Instance.Datacenter.MonstersData.GetMonsterDataById(pair.Key);
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

        public PetData()
        {
            Foods = new();
        }

        public ItemData? GetItemData()
        {
            return DofusApi.Instance.Datacenter.ItemsData.GetItemDataById(Id);
        }
    }

    public class PetsData
    {
        private const string FILE_NAME = "pets.json";

        [JsonPropertyName("PET")]
        public List<PetData> Pets { get; init; }

        public PetsData()
        {
            Pets = new();
        }

        internal static PetsData Build()
        {
            return Json.LoadFromFile<PetsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public PetData? GetPetDataByItemId(int id)
        {
            return Pets.Find(x => x.Id == id);
        }
    }
}
