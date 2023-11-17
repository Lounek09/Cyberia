using Cyberia.Api.Factories.Effects;
using Cyberia.Api.JsonConverters;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Custom
{
    internal sealed class ItemSetCustomData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("e")]
        [JsonConverter(typeof(ItemEffectListListConverter))]
        public List<List<IEffect>> Effects { get; init; }

        [JsonConstructor]
        internal ItemSetCustomData()
        {
            Effects = [];
        }
    }

    internal sealed class ItemSetsCustomData
    {
        [JsonPropertyName("CIS")]
        public List<ItemSetCustomData> ItemSetsCustom { get; init; }

        [JsonConstructor]
        internal ItemSetsCustomData()
        {
            ItemSetsCustom = [];
        }
    }
}
