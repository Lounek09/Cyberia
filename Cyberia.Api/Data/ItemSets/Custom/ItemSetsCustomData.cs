using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.ItemSets.Custom;

internal sealed class ItemSetsCustomData : IDofusData
{
    [JsonPropertyName("CIS")]
    public List<ItemSetCustomData> ItemSetsCustom { get; init; }

    [JsonConstructor]
    internal ItemSetsCustomData()
    {
        ItemSetsCustom = [];
    }
}
