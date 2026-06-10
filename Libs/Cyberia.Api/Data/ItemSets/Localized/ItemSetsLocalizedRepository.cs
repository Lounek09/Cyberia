using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.ItemSets.Localized;

internal sealed class ItemSetsLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => ItemSetsRepository.FileName;

    [JsonPropertyName("IS")]
    public IReadOnlyList<ItemSetLocalizedData> ItemSets { get; init; }

    [JsonConstructor]
    internal ItemSetsLocalizedRepository()
    {
        ItemSets = ReadOnlyCollection<ItemSetLocalizedData>.Empty;
    }
}
