using Cyberia.Api.Factories.Effects;
using Cyberia.Api.Parser.JsonConverter;

using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class ItemStatsData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        [JsonConverter(typeof(ItemEffectListJsonConverter))]
        public List<IEffect> Effects { get; init; }

        public ItemStatsData()
        {
            Effects = new();
        }
    }

    public sealed class ItemsStatsData
    {
        private const string FILE_NAME = "itemstats.json";

        [JsonPropertyName("ISTA")]
        public List<ItemStatsData> ItemsStats { get; init; }

        public ItemsStatsData()
        {
            ItemsStats = new();
        }

        internal static ItemsStatsData Build()
        {
            return Json.LoadFromFile<ItemsStatsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public ItemStatsData? GetItemStatDataById(int id)
        {
            return ItemsStats.Find(x => x.Id == id);
        }
    }
}
