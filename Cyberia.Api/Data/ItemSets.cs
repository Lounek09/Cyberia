using Cyberia.Api.Data.Custom;
using Cyberia.Api.Factories.Effects;
using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class ItemSetData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("i")]
        [JsonConverter(typeof(ReadOnlyCollectionConverter<int>))]
        public ReadOnlyCollection<int> ItemsId { get; init; }

        [JsonIgnore]
        public ReadOnlyCollection<IEnumerable<IEffect>> Effects { get; internal set; }

        [JsonConstructor]
        internal ItemSetData()
        {
            Name = string.Empty;
            ItemsId = ReadOnlyCollection<int>.Empty;
            Effects = ReadOnlyCollection<IEnumerable<IEffect>>.Empty;
        }

        public IEnumerable<ItemData> GetItemsData()
        {
            foreach (int id in ItemsId)
            {
                ItemData? itemData = DofusApi.Datacenter.ItemsData.GetItemDataById(id);
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

        public IEnumerable<IEffect> GetEffects(int nbItem)
        {
            int index = nbItem - 2;

            return Effects.Count > index && index >= 0 ? Effects[index] : [];
        }

        public BreedData? GetBreedData()
        {
            return DofusApi.Datacenter.BreedsData.Breeds.Values.FirstOrDefault(x => x.ItemSetId == Id);
        }
    }

    public sealed class ItemSetsData : IDofusData
    {
        private const string FILE_NAME = "itemsets.json";

        [JsonPropertyName("IS")]
        [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ItemSetData>))]
        public FrozenDictionary<int, ItemSetData> ItemSets { get; init; }

        [JsonConstructor]
        internal ItemSetsData()
        {
            ItemSets = FrozenDictionary<int, ItemSetData>.Empty;
        }

        internal static ItemSetsData Load()
        {
            ItemSetsData data = Datacenter.LoadDataFromFile<ItemSetsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
            ItemSetsCustomData customData = Datacenter.LoadDataFromFile<ItemSetsCustomData>(Path.Combine(DofusApi.CUSTOM_PATH, FILE_NAME));

            foreach (ItemSetCustomData itemSetCustomData in customData.ItemSetsCustom)
            {
                ItemSetData? itemSetData = data.GetItemSetDataById(itemSetCustomData.Id);
                if (itemSetData is not null)
                {
                    itemSetData.Effects = itemSetCustomData.Effects.AsReadOnly();
                }
            }

            return data;
        }

        public ItemSetData? GetItemSetDataById(int id)
        {
            ItemSets.TryGetValue(id, out ItemSetData? itemSetData);
            return itemSetData;
        }

        public string GetItemSetNameById(int id)
        {
            ItemSetData? itemSetData = GetItemSetDataById(id);

            return itemSetData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : itemSetData.Name;
        }

        public IEnumerable<ItemSetData> GetItemSetsDataByName(string name)
        {
            string[] names = name.NormalizeCustom().Split(' ');
            return ItemSets.Values.Where(x => names.All(x.Name.NormalizeCustom().Contains));
        }
    }
}
