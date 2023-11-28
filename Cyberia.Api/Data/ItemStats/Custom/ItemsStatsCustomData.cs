using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.ItemStats.Custom;

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
