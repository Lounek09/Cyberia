using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class KnowledgeBookCatagoryData
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

    public sealed class KnowledgeBookArticleData
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

    public sealed class KnowledgeBookTipData
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

    public sealed class KnowledgeBookTriggerData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("t")]
        public int Type { get; init; }

        [JsonPropertyName("v")]
        //TODO: jsonconverter for Values in KnowledgeBookTrigger
        public object Values { get; init; }

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
        public List<KnowledgeBookCatagoryData> KnowledgeBookCatagories { get; init; }

        [JsonPropertyName("KBA")]
        public List<KnowledgeBookArticleData> KnowledgeBookArticles { get; init; }

        [JsonPropertyName("KBT")]
        public List<KnowledgeBookTipData> KnowledgeBookTips { get; init; }

        [JsonPropertyName("KBD")]
        public List<KnowledgeBookTriggerData> KnowledgeBookTriggers { get; init; }

        [JsonConstructor]
        internal KnowledgeBookData()
        {
            KnowledgeBookCatagories = [];
            KnowledgeBookArticles = [];
            KnowledgeBookTips = [];
            KnowledgeBookTriggers = [];
        }

        internal static KnowledgeBookData Load()
        {
            return Datacenter.LoadDataFromFile<KnowledgeBookData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public KnowledgeBookCatagoryData? GetKnowledgeBookCatagoryDataById(int id)
        {
            return KnowledgeBookCatagories.Find(x => x.Id == id);
        }

        public KnowledgeBookArticleData? GetKnowledgeBookArticleDataById(int id)
        {
            return KnowledgeBookArticles.Find(x => x.Id == id);
        }

        public KnowledgeBookTipData? GetKnowledgeBookTipDataById(int id)
        {
            return KnowledgeBookTips.Find(x => x.Id == id);
        }
    }
}
