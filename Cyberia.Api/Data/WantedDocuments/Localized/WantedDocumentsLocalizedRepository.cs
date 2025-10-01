using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.WantedDocuments.Localized;

internal sealed class WantedDocumentsLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => WantedDocumentsRepository.FileName;

    [JsonPropertyName("DW")]
    public IReadOnlyList<WantedDocumentLocalizedData> WantedDocuments { get; init; }

    [JsonConstructor]
    internal WantedDocumentsLocalizedRepository()
    {
        WantedDocuments = ReadOnlyCollection<WantedDocumentLocalizedData>.Empty;
    }
}
