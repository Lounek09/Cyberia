using Cyberia.Api.Factories.Effects;
using Cyberia.Api.JsonConverters;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    internal sealed class ItemStatsCustomData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        [JsonConverter(typeof(ItemEffectListConverter))]
        public List<IEffect> Effects { get; init; }

        [JsonConstructor]
        internal ItemStatsCustomData()
        {
            Effects = [];
        }

        public ItemStatsData ToItemStatsData()
        {
            return new()
            {
                Id = Id,
                EffectsCore = Effects
            };
        }
    }

    internal sealed class ItemsStatsCustomData : IDofusData
    {

        [JsonPropertyName("CISTA")]
        public List<ItemStatsCustomData> ItemsStatsCustom { get; init; }

        [JsonConstructor]
        internal ItemsStatsCustomData()
        {
            ItemsStatsCustom = [];
        }
    }
}
