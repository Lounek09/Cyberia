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
        [JsonInclude]
        internal List<ItemStatsData> ItemsStatsCore { get; init; }

        [JsonIgnore]
        public FrozenDictionary<int, ItemStatsData> ItemsStats { get; internal set; }

        [JsonConstructor]
        internal ItemsStatsData()
        {
            ItemsStatsCore = [];
            ItemsStats = FrozenDictionary<int, ItemStatsData>.Empty;
        }

        internal static ItemsStatsData Load()
        {
            ItemsStatsData data = Datacenter.LoadDataFromFile<ItemsStatsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
            ItemsStatsCustomData customData = Datacenter.LoadDataFromFile<ItemsStatsCustomData>(Path.Combine(DofusApi.CUSTOM_PATH, FILE_NAME));

            foreach (ItemStatsCustomData itemStatsCustomData in customData.ItemsStatsCustom)
            {
                ItemStatsData? itemStatsData = data.ItemsStatsCore.Find(x => x.Id == itemStatsCustomData.Id);
                if (itemStatsData is not null)
                {
                    itemStatsData.EffectsCore.AddRange(itemStatsCustomData.Effects);
                    continue;
                }

                data.ItemsStatsCore.Add(itemStatsCustomData.ToItemStatsData());
            }

            data.ItemsStats = data.ItemsStatsCore.GroupBy(x => x.Id).ToFrozenDictionary(x => x.Key, x => x.ElementAt(0));
            return data;
        }

        public ItemStatsData? GetItemStatDataById(int id)
        {
            ItemsStats.TryGetValue(id, out ItemStatsData? itemStatsData);
            return itemStatsData;
        }
    }
}
