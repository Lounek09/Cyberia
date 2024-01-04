using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.KnowledgeBook;

public sealed class KnowledgeBookArticleData
    : IDofusData<int>
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
