using Cyberia.Api.Data.KnowledgeBook.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Primitives;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.KnowledgeBook;

public sealed class KnowledgeBookRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "kb.json";

    [JsonPropertyName("KBC")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, KnowledgeBookCatagoryData>))]
    public FrozenDictionary<int, KnowledgeBookCatagoryData> KnowledgeBookCatagories { get; init; }

    [JsonPropertyName("KBA")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, KnowledgeBookArticleData>))]
    public FrozenDictionary<int, KnowledgeBookArticleData> KnowledgeBookArticles { get; init; }

    [JsonPropertyName("KBT")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, KnowledgeBookTipData>))]
    public FrozenDictionary<int, KnowledgeBookTipData> KnowledgeBookTips { get; init; }

    [JsonPropertyName("KBD")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, KnowledgeBookTriggerData>))]
    public FrozenDictionary<int, KnowledgeBookTriggerData> KnowledgeBookTriggers { get; init; }

    [JsonConstructor]
    internal KnowledgeBookRepository()
    {
        KnowledgeBookCatagories = FrozenDictionary<int, KnowledgeBookCatagoryData>.Empty;
        KnowledgeBookArticles = FrozenDictionary<int, KnowledgeBookArticleData>.Empty;
        KnowledgeBookTips = FrozenDictionary<int, KnowledgeBookTipData>.Empty;
        KnowledgeBookTriggers = FrozenDictionary<int, KnowledgeBookTriggerData>.Empty;
    }

    public KnowledgeBookCatagoryData? GetKnowledgeBookCatagoryDataById(int id)
    {
        KnowledgeBookCatagories.TryGetValue(id, out var knowledgeBookCatagoryData);
        return knowledgeBookCatagoryData;
    }

    public KnowledgeBookArticleData? GetKnowledgeBookArticleDataById(int id)
    {
        KnowledgeBookArticles.TryGetValue(id, out var knowledgeBookArticleData);
        return knowledgeBookArticleData;
    }

    public KnowledgeBookTipData? GetKnowledgeBookTipDataById(int id)
    {
        KnowledgeBookTips.TryGetValue(id, out var knowledgeBookTipData);
        return knowledgeBookTipData;
    }

    protected override void LoadLocalizedData(LangsIdentifier identifier)
    {
        var twoLetterISOLanguageName = identifier.Language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<KnowledgeBookLocalizedRepository>(identifier);

        foreach (var knowledgeBookCatagoryLocalizedData in localizedRepository.KnowledgeBookCatagories)
        {
            var knowledgeBookCatagoryData = GetKnowledgeBookCatagoryDataById(knowledgeBookCatagoryLocalizedData.Id);
            knowledgeBookCatagoryData?.Name.TryAdd(twoLetterISOLanguageName, knowledgeBookCatagoryLocalizedData.Name);
        }

        foreach (var knowledgeBookArticleLocalizedData in localizedRepository.KnowledgeBookArticles)
        {
            var knowledgeBookArticleData = GetKnowledgeBookArticleDataById(knowledgeBookArticleLocalizedData.Id);
            if (knowledgeBookArticleData is not null)
            {
                knowledgeBookArticleData.Name.TryAdd(twoLetterISOLanguageName, knowledgeBookArticleLocalizedData.Name);
                knowledgeBookArticleData.Description.TryAdd(twoLetterISOLanguageName, knowledgeBookArticleLocalizedData.Description);

                if (knowledgeBookArticleLocalizedData.KeyWords.Count == knowledgeBookArticleData.KeyWords.Count)
                {
                    for (var i = 0; i < knowledgeBookArticleLocalizedData.KeyWords.Count; i++)
                    {
                        knowledgeBookArticleData.KeyWords[i].TryAdd(twoLetterISOLanguageName, knowledgeBookArticleLocalizedData.KeyWords[i]);
                    }
                }
            }
        }
    }
}
