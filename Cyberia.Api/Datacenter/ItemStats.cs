using Cyberia.Api.Factories.Effects;
using Cyberia.Api.Parser.JsonConverter;

using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class ItemStats
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        [JsonConverter(typeof(ItemEffectsJsonConverter))]
        public List<IEffect> Effects { get; init; }

        public ItemStats()
        {
            Effects = new();
        }
    }

    public sealed class ItemStatsData
    {
        private const string FILE_NAME = "itemstats.json";

        [JsonPropertyName("ISTA")]
        public List<ItemStats> ItemsStats { get; init; }

        public ItemStatsData()
        {
            ItemsStats = new();
        }

        internal static ItemStatsData Build()
        {
            return Json.LoadFromFile<ItemStatsData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
        }

        public ItemStats? GetItemStatById(int id)
        {
            return ItemsStats.Find(x => x.Id == id);
        }
    }
}
