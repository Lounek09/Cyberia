using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.KnowledgeBook;

public sealed class KnowledgeBookData
    : IDofusData
{
    private const string FILE_NAME = "kb.json";

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
    internal KnowledgeBookData()
    {
        KnowledgeBookCatagories = FrozenDictionary<int, KnowledgeBookCatagoryData>.Empty;
        KnowledgeBookArticles = FrozenDictionary<int, KnowledgeBookArticleData>.Empty;
        KnowledgeBookTips = FrozenDictionary<int, KnowledgeBookTipData>.Empty;
        KnowledgeBookTriggers = FrozenDictionary<int, KnowledgeBookTriggerData>.Empty;
    }

    internal static KnowledgeBookData Load()
    {
        return Datacenter.LoadDataFromFile<KnowledgeBookData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
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
}
