using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class KnowledgeBookCatagory
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

        public KnowledgeBookCatagory()
        {
            Name = string.Empty;
        }
    }

    public sealed class KnowledgeBookArticle
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

        public KnowledgeBookArticle()
        {
            Name = string.Empty;
            Description = string.Empty;
            KeyWords = new();
        }

        public KnowledgeBookCatagory? GetKnowledgeBookCatagory()
        {
            return DofusApi.Instance.Datacenter.KnowledgeBookData.GetKnowledgeBookCatagoryById(KnowledgeBookCatagoryId);
        }
    }

    public sealed class KnowledgeBookTip
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("i")]
        public int Index { get; init; }

        [JsonPropertyName("c")]
        public string Description { get; init; }

        [JsonPropertyName("l")]
        public int KnowledgeBookArticleId { get; init; }

        public KnowledgeBookTip()
        {
            Description = string.Empty;
        }

        public KnowledgeBookArticle? GetKnowledgeBookArticle()
        {
            return DofusApi.Instance.Datacenter.KnowledgeBookData.GetKnowledgeBookArticleById(KnowledgeBookArticleId);
        }
    }

    public sealed class KnowledgeBookTrigger
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

        public KnowledgeBookTrigger()
        {
            Values = new();
        }

        public KnowledgeBookTip? GetKnowledgeBookTip()
        {
            return DofusApi.Instance.Datacenter.KnowledgeBookData.GetKnowledgeBookTipById(KnowledgeBookTipId);
        }
    }

    public sealed class KnowledgeBookData
    {
        private const string FILE_NAME = "kb.json";

        [JsonPropertyName("KBC")]
        public List<KnowledgeBookCatagory> KnowledgeBookCatagories { get; init; }

        [JsonPropertyName("KBA")]
        public List<KnowledgeBookArticle> KnowledgeBookArticles { get; init; }

        [JsonPropertyName("KBT")]
        public List<KnowledgeBookTip> KnowledgeBookTips { get; init; }

        [JsonPropertyName("KBD")]
        public List<KnowledgeBookTrigger> KnowledgeBookTriggers { get; init; }

        public KnowledgeBookData()
        {
            KnowledgeBookCatagories = new();
            KnowledgeBookArticles = new();
            KnowledgeBookTips = new();
            KnowledgeBookTriggers = new();
        }

        internal static KnowledgeBookData Build()
        {
            return Json.LoadFromFile<KnowledgeBookData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
        }

        public KnowledgeBookCatagory? GetKnowledgeBookCatagoryById(int id)
        {
            return KnowledgeBookCatagories.Find(x => x.Id == id);
        }

        public KnowledgeBookArticle? GetKnowledgeBookArticleById(int id)
        {
            return KnowledgeBookArticles.Find(x => x.Id == id);
        }

        public KnowledgeBookTip? GetKnowledgeBookTipById(int id)
        {
            return KnowledgeBookTips.Find(x => x.Id == id);
        }
    }
}
