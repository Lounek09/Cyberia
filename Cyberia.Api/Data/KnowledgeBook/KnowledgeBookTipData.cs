using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.KnowledgeBook;

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
        return DofusApi.Datacenter.KnowledgeBookRepository.GetKnowledgeBookArticleDataById(KnowledgeBookArticleId);
    }
}
