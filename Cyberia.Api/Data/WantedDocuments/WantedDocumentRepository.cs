using Cyberia.Api.Data.TTG.Localized;
using Cyberia.Api.Data.WantedDocuments.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.WantedDocuments;

public sealed class WantedDocumentsRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "wanteddocument.json";

    [JsonPropertyName("DW")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, WantedDocumentData>))]
    public FrozenDictionary<int, WantedDocumentData> WantedDocuments { get; init; }

    [JsonConstructor]
    internal WantedDocumentsRepository()
    {
        WantedDocuments = FrozenDictionary<int, WantedDocumentData>.Empty;
    }

    public WantedDocumentData? GetWantedDocumentDataById(int id)
    {
        WantedDocuments.TryGetValue(id, out var wantedDocumentData);
        return wantedDocumentData;
    }

    protected override void LoadLocalizedData(LangsIdentifier identifier)
    {
        var twoLetterISOLanguageName = identifier.Language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<WantedDocumentsLocalizedRepository>(identifier);

        foreach (var wantedDocumentLocalizedData in localizedRepository.WantedDocuments)
        {
            var wantedDocumentData = GetWantedDocumentDataById(wantedDocumentLocalizedData.Id);
            if (wantedDocumentData is not null)
            {
                wantedDocumentData.Body.TryAdd(twoLetterISOLanguageName, wantedDocumentLocalizedData.Body);
                wantedDocumentData.Description.TryAdd(twoLetterISOLanguageName, wantedDocumentLocalizedData.Description);
            }
        }
    }
}
