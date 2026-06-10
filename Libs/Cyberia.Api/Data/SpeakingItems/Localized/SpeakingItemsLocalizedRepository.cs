using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.SpeakingItems.Localized;

internal sealed class SpeakingItemsLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => SpeakingItemsRepository.FileName;

    [JsonPropertyName("SIM")]
    public IReadOnlyList<SpeakingItemsMessageLocalizedData> SpeakingItemsMessages { get; init; }

    [JsonConstructor]
    internal SpeakingItemsLocalizedRepository()
    {
        SpeakingItemsMessages = ReadOnlyCollection<SpeakingItemsMessageLocalizedData>.Empty;
    }
}
