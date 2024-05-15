using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.ItemStats.Custom;

internal sealed class ItemsStatsCustomRepository : IDofusRepository
{
    [JsonPropertyName("CISTA")]
    public IReadOnlyList<ItemStatsCustomData> ItemsStatsCustom { get; init; }

    [JsonConstructor]
    internal ItemsStatsCustomRepository()
    {
        ItemsStatsCustom = [];
    }
}
