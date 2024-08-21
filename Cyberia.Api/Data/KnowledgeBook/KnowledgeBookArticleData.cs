using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.KnowledgeBook;

public sealed class KnowledgeBookArticleData : IDofusData<int>, IComparable<KnowledgeBookArticleData>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("c")]
    public int KnowledgeBookCatagoryId { get; init; }

    [JsonPropertyName("n")]
    public LocalizedString Name { get; init; }

    [JsonPropertyName("o")]
    public int Order { get; init; }

    [JsonPropertyName("a")]
    public LocalizedString Description { get; init; }

    [JsonPropertyName("k")]
    public List<LocalizedString> KeyWords { get; init; }

    [JsonPropertyName("i")]
    public int Index { get; init; }

    [JsonPropertyName("ep")]
    public int Episode { get; init; }

    [JsonConstructor]
    internal KnowledgeBookArticleData()
    {
        Name = LocalizedString.Empty;
        Description = LocalizedString.Empty;
        KeyWords = [];
    }

    public KnowledgeBookCatagoryData? GetKnowledgeBookCatagoryData()
    {
        return DofusApi.Datacenter.KnowledgeBookRepository.GetKnowledgeBookCatagoryDataById(KnowledgeBookCatagoryId);
    }

    public int CompareTo(KnowledgeBookArticleData? other)
    {
        if (other is null)
        {
            return 1;
        }

        return Order.CompareTo(other.Order);
    }
}
