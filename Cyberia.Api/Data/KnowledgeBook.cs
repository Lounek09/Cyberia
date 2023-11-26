using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class KnowledgeBookCatagoryData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("o")]
        public int Order { get; init; }

        [JsonPropertyName("i")]
        public int Index { get; init; }

        [JsonPropertyName("ep")]
        public int Episode { get; init; }

        [JsonConstructor]
        internal KnowledgeBookCatagoryData()
        {
            Name = string.Empty;
        }
    }

    public sealed class KnowledgeBookArticleData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("c")]
        public int KnowledgeBookCatagoryId { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("o")]
        public int Order { get; init; }

        [JsonPropertyName("a")]
        public string Description { get; init; }

        [JsonPropertyName("k")]
        public List<string> KeyWords { get; init; }

        [JsonPropertyName("i")]
        public int Index { get; init; }

        [JsonPropertyName("ep")]
        public int Episode { get; init; }

        [JsonConstructor]
        internal KnowledgeBookArticleData()
        {
            Name = string.Empty;
            Description = string.Empty;
            KeyWords = [];
        }

        public KnowledgeBookCatagoryData? GetKnowledgeBookCatagoryData()
        {
            return DofusApi.Datacenter.KnowledgeBookData.GetKnowledgeBookCatagoryDataById(KnowledgeBookCatagoryId);
        }
    }

    public sealed class KnowledgeBookTipData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("i")]
        public int Index { get; init; }

        [JsonPropertyName("c")]
        public string Description { get; init; }

        [JsonPropertyName("l")]
        public int KnowledgeBookArticleId { get; init; }

        [JsonConstructor]
        internal KnowledgeBookTipData()
        {
            Description = string.Empty;
        }

        public KnowledgeBookArticleData? GetKnowledgeBookArticleData()
        {
            return DofusApi.Datacenter.KnowledgeBookData.GetKnowledgeBookArticleDataById(KnowledgeBookArticleId);
        }
    }

    public sealed class KnowledgeBookTriggerData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("t")]
        public int Type { get; init; }

        [JsonPropertyName("v")]
        [JsonInclude]
        //TODO: JsonConverter for Values in KnowledgeBookTrigger
        internal object Values { get; init; }

        [JsonPropertyName("d")]
        public int KnowledgeBookTipId { get; init; }

        [JsonConstructor]
        internal KnowledgeBookTriggerData()
        {
            Values = new();
        }

        public KnowledgeBookTipData? GetKnowledgeBookTipData()
        {
            return DofusApi.Datacenter.KnowledgeBookData.GetKnowledgeBookTipDataById(KnowledgeBookTipId);
        }
    }

    public sealed class KnowledgeBookData
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
            KnowledgeBookCatagories.TryGetValue(id, out KnowledgeBookCatagoryData? knowledgeBookCatagoryData);
            return knowledgeBookCatagoryData;
        }

        public KnowledgeBookArticleData? GetKnowledgeBookArticleDataById(int id)
        {
            KnowledgeBookArticles.TryGetValue(id, out KnowledgeBookArticleData? knowledgeBookArticleData);
            return knowledgeBookArticleData;
        }

        public KnowledgeBookTipData? GetKnowledgeBookTipDataById(int id)
        {
            KnowledgeBookTips.TryGetValue(id, out KnowledgeBookTipData? knowledgeBookTipData);
            return knowledgeBookTipData;
        }
    }
}
