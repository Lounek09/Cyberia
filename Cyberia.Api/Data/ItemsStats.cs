using Cyberia.Api.Factories.Effects;
using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class ItemStatsData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        [JsonConverter(typeof(ItemEffectListConverter))]
        [JsonInclude]
        internal List<IEffect> EffectsCore { get; init; }

        [JsonIgnore]
        public ReadOnlyCollection<IEffect> Effects => EffectsCore.AsReadOnly();

        [JsonConstructor]
        internal ItemStatsData()
        {
            EffectsCore = [];
        }
    }

    public sealed class ItemsStatsData : IDofusData
    {
        private const string FILE_NAME = "itemstats.json";

        [JsonPropertyName("ISTA")]
        [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ItemStatsData>))]
        [JsonInclude]
        public FrozenDictionary<int, ItemStatsData> ItemsStats { get; internal set; }

        [JsonConstructor]
        internal ItemsStatsData()
        {
            ItemsStats = FrozenDictionary<int, ItemStatsData>.Empty;
        }

        internal static ItemsStatsData Load()
        {
            ItemsStatsData data = Datacenter.LoadDataFromFile<ItemsStatsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
            ItemsStatsCustomData customData = Datacenter.LoadDataFromFile<ItemsStatsCustomData>(Path.Combine(DofusApi.CUSTOM_PATH, FILE_NAME));

            Dictionary<int, ItemStatsData> tempItemsStats = data.ItemsStats.ToDictionary();
            foreach (ItemStatsCustomData itemStatsCustomData in customData.ItemsStatsCustom)
            {
                ItemStatsData? itemStatsData = data.GetItemStatDataById(itemStatsCustomData.Id);
                if (itemStatsData is not null)
                {
                    itemStatsData.EffectsCore.AddRange(itemStatsCustomData.Effects);
                    continue;
                }

                tempItemsStats.Add(itemStatsCustomData.Id, itemStatsCustomData.ToItemStatsData());
            }

            data.ItemsStats = tempItemsStats.ToFrozenDictionary();
            return data;
        }

        public ItemStatsData? GetItemStatDataById(int id)
        {
            ItemsStats.TryGetValue(id, out ItemStatsData? itemStatsData);
            return itemStatsData;
        }
    }
}
