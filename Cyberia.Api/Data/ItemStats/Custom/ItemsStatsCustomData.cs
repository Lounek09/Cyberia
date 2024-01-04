using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.ItemStats.Custom;

internal sealed class ItemsStatsCustomData
    : IDofusData
{

    [JsonPropertyName("CISTA")]
    public IReadOnlyList<ItemStatsCustomData> ItemsStatsCustom { get; init; }

    [JsonConstructor]
    internal ItemsStatsCustomData()
    {
        ItemsStatsCustom = [];
    }
}
