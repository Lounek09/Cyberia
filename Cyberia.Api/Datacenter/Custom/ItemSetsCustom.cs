using Cyberia.Api.Factories.Effects;
using Cyberia.Api.Parser.JsonConverter;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Custom
{
    internal sealed class ItemSetCustomData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("e")]
        [JsonConverter(typeof(ItemEffectListListJsonConverter))]
        public List<List<IEffect>> Effects { get; init; }

        public ItemSetCustomData()
        {
            Effects = new();
        }
    }

    internal sealed class ItemSetsCustomData
    {
        [JsonPropertyName("CIS")]
        public List<ItemSetCustomData> ItemSetsCustom { get; init; }

        public ItemSetsCustomData()
        {
            ItemSetsCustom = new();
        }
    }
}
