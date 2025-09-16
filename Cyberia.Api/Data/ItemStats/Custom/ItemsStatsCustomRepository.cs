using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.ItemStats.Custom;

internal sealed class ItemsStatsCustomRepository : DofusCustomRepository, IDofusRepository
{
    public static string FileName => ItemsStatsRepository.FileName;

    [JsonPropertyName("CISTA")]
    public IReadOnlyList<ItemStatsCustomData> ItemsStatsCustom { get; init; }

    [JsonConstructor]
    internal ItemsStatsCustomRepository()
    {
        ItemsStatsCustom = ReadOnlyCollection<ItemStatsCustomData>.Empty;
    }
}
