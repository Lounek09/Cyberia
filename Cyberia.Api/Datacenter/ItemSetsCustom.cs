using Cyberia.Api.Factories.Effects;
using Cyberia.Api.Factories.JsonConverter;

using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    internal sealed class ItemSetCustom
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("e")]
        [JsonConverter(typeof(ItemEffectsListJsonConverter))]
        public List<List<IEffect>> Effects { get; init; }

        public ItemSetCustom()
        {
            Effects = new();
        }
    }

    internal sealed class ItemSetsCustomData
    {
        [JsonPropertyName("CIS")]
        public List<ItemSetCustom> ItemSetsCustom { get; init; }

        public ItemSetsCustomData()
        {
            ItemSetsCustom = new();
        }
    }
}
