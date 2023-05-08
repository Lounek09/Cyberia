using Cyberia.Api.Factories.Effects;

using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class ItemSet
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("i")]
        public List<int> ItemsId { get; init; }

        public List<List<IEffect>> Effects { get; internal set; }

        public ItemSet()
        {
            Name = string.Empty;
            ItemsId = new();
            Effects = new();
        }

        public List<Item> GetItems()
        {
            List<Item> items = new();

            foreach (int id in ItemsId)
            {
                Item? item = DofusApi.Instance.Datacenter.ItemsData.GetItemById(id);
                if (item is not null)
                    items.Add(item);
            }

            return items;
        }

        public int GetLevel()
        {
            int level = 0;

            foreach (Item item in GetItems())
                level = item.Level > level ? item.Level : level;

            return level;
        }

        public List<IEffect> GetEffects(int nbItem)
        {
            int index = nbItem - 2;

            return Effects.Count > index && index >= 0 ? Effects[index] : new();
        }

        public Breed? GetBreed()
        {
            return DofusApi.Instance.Datacenter.BreedsData.Breeds.Find(x => x.ItemSetId == Id);
        }
    }

    public sealed class ItemSetsData
    {
        private const string FILE_NAME = "itemsets.json";

        [JsonPropertyName("IS")]
        public List<ItemSet> ItemSets { get; init; }

        public ItemSetsData()
        {
            ItemSets = new();
        }

        internal static ItemSetsData Build()
        {
            ItemSetsData data = Json.LoadFromFile<ItemSetsData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
            ItemSetsCustomData customData = Json.LoadFromFile<ItemSetsCustomData>($"{DofusApi.CUSTOM_PATH}/{FILE_NAME}");

            foreach (ItemSetCustom itemSetCustom in customData.ItemSetsCustom)
            {
                ItemSet? itemSet = data.GetItemSetById(itemSetCustom.Id);
                if (itemSet is not null)
                    itemSet.Effects = itemSetCustom.Effects;
            }

            return data;
        }

        public ItemSet? GetItemSetById(int id)
        {
            return ItemSets.Find(x => x.Id == id);
        }

        public string GetItemSetNameById(int id)
        {
            ItemSet? itemSet = GetItemSetById(id);

            return itemSet is null ? $"Inconnu ({id})" : itemSet.Name;
        }

        public ItemSet? GetItemSetByName(string name)
        {
            return ItemSets.Find(x => x.Name.RemoveDiacritics().Equals(name.RemoveDiacritics()));
        }

        public List<ItemSet> GetItemSetsByName(string name)
        {
            string[] names = name.RemoveDiacritics().Split(' ');
            return ItemSets.FindAll(x => names.All(x.Name.RemoveDiacritics().Contains));
        }
    }
}
