using Cyberia.Api.DatacenterNS.Custom;
using Cyberia.Api.Factories.Effects;

using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class ItemSetData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("i")]
        public List<int> ItemsId { get; init; }

        public List<List<IEffect>> Effects { get; internal set; }

        public ItemSetData()
        {
            Name = string.Empty;
            ItemsId = new();
            Effects = new();
        }

        public IEnumerable<ItemData> GetItemsData()
        {
            foreach (int id in ItemsId)
            {
                ItemData? itemData = DofusApi.Instance.Datacenter.ItemsData.GetItemDataById(id);
                if (itemData is not null)
                {
                    yield return itemData;
                }
            }
        }

        public int GetLevel()
        {
            return GetItemsData().Max(x => x.Level);
        }

        public List<IEffect> GetEffects(int nbItem)
        {
            int index = nbItem - 2;

            return Effects.Count > index && index >= 0 ? Effects[index] : new();
        }

        public BreedData? GetBreedData()
        {
            return DofusApi.Instance.Datacenter.BreedsData.Breeds.Find(x => x.ItemSetId == Id);
        }
    }

    public sealed class ItemSetsData
    {
        private const string FILE_NAME = "itemsets.json";

        [JsonPropertyName("IS")]
        public List<ItemSetData> ItemSets { get; init; }

        public ItemSetsData()
        {
            ItemSets = new();
        }

        internal static ItemSetsData Build()
        {
            ItemSetsData data = Json.LoadFromFile<ItemSetsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
            ItemSetsCustomData customData = Json.LoadFromFile<ItemSetsCustomData>(Path.Combine(DofusApi.CUSTOM_PATH, FILE_NAME));

            foreach (ItemSetCustomData itemSetCustomData in customData.ItemSetsCustom)
            {
                ItemSetData? itemSetData = data.GetItemSetDataById(itemSetCustomData.Id);
                if (itemSetData is not null)
                {
                    itemSetData.Effects = itemSetCustomData.Effects;
                }
            }

            return data;
        }

        public ItemSetData? GetItemSetDataById(int id)
        {
            return ItemSets.Find(x => x.Id == id);
        }

        public string GetItemSetNameById(int id)
        {
            ItemSetData? itemSetData = GetItemSetDataById(id);

            return itemSetData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : itemSetData.Name;
        }

        public ItemSetData? GetItemSetByName(string name)
        {
            return ItemSets.Find(x => ExtendString.Normalize(x.Name).Equals(ExtendString.Normalize(name)));
        }

        public List<ItemSetData> GetItemSetsDataByName(string name)
        {
            string[] names = ExtendString.Normalize(name).Split(' ');
            return ItemSets.FindAll(x => names.All(ExtendString.Normalize(x.Name).Contains));
        }
    }
}
