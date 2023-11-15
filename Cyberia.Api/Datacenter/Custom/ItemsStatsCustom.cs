using Cyberia.Api.Factories.Effects;
using Cyberia.Api.Parser.JsonConverter;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    internal sealed class ItemStatsCustomData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        [JsonConverter(typeof(ItemEffectListJsonConverter))]
        public List<IEffect> Effects { get; init; }

        public ItemStatsCustomData()
        {
            Effects = [];
        }

        public ItemStatsData ToItemStatsData()
        {
            return new()
            {
                Id = Id,
                Effects = Effects
            };
        }
    }

    internal sealed class ItemsStatsCustomData
    {

        [JsonPropertyName("CISTA")]
        public List<ItemStatsCustomData> ItemsStatsCustom { get; init; }

        public ItemsStatsCustomData()
        {
            ItemsStatsCustom = [];
        }
    }
}
