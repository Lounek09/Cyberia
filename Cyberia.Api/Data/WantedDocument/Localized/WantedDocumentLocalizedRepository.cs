using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.WantedDocument.Localized;

internal sealed class WantedDocumentLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => WantedDocumentRepository.FileName;

    [JsonPropertyName("DW")]
    public IReadOnlyList<WantedDocumentLocalizedData> WantedDocuments { get; init; }

    [JsonConstructor]
    internal WantedDocumentLocalizedRepository()
    {
        WantedDocuments = ReadOnlyCollection<WantedDocumentLocalizedData>.Empty;
    }
}
