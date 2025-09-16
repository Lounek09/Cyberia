using Cyberia.Api.Data.TTG.Localized;
using Cyberia.Api.Data.WantedDocument.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.WantedDocument;

public sealed class WantedDocumentRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "wanteddocument.json";

    [JsonPropertyName("DW")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, WantedDocumentData>))]
    public FrozenDictionary<int, WantedDocumentData> WantedDocuments { get; init; }

    [JsonConstructor]
    internal WantedDocumentRepository()
    {
        WantedDocuments = FrozenDictionary<int, WantedDocumentData>.Empty;
    }

    public WantedDocumentData? GetWantedDocumentDataById(int id)
    {
        WantedDocuments.TryGetValue(id, out var wantedDocumentData);
        return wantedDocumentData;
    }

    protected override void LoadLocalizedData(LangType type, Language language)
    {
        var twoLetterISOLanguageName = language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<WantedDocumentLocalizedRepository>(type, language);

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
