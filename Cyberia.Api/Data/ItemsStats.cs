using Cyberia.Api.Factories.Effects;
using Cyberia.Api.JsonConverters;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class ItemStatsData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        [JsonConverter(typeof(ItemEffectListConverter))]
        public List<IEffect> Effects { get; init; }

        [JsonConstructor]
        internal ItemStatsData()
        {
            Effects = [];
        }
    }

    public sealed class ItemsStatsData
    {
        private const string FILE_NAME = "itemstats.json";

        [JsonPropertyName("ISTA")]
        public List<ItemStatsData> ItemsStats { get; init; }

        [JsonConstructor]
        internal ItemsStatsData()
        {
            ItemsStats = [];
        }

        internal static ItemsStatsData Load()
        {
            ItemsStatsData data = Datacenter.LoadDataFromFile<ItemsStatsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
            ItemsStatsCustomData customData = Datacenter.LoadDataFromFile<ItemsStatsCustomData>(Path.Combine(DofusApi.CUSTOM_PATH, FILE_NAME));

            foreach (ItemStatsCustomData itemStatsCustomData in customData.ItemsStatsCustom)
            {
                ItemStatsData? itemStatsData = data.GetItemStatDataById(itemStatsCustomData.Id);
                if (itemStatsData is not null)
                {
                    itemStatsData.Effects.AddRange(itemStatsCustomData.Effects);
                    continue;
                }

                data.ItemsStats.Add(itemStatsCustomData.ToItemStatsData());
            }

            return data;
        }

        public ItemStatsData? GetItemStatDataById(int id)
        {
            return ItemsStats.Find(x => x.Id == id);
        }
    }
}
