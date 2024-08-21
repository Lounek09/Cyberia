using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.KnowledgeBook;

public sealed class KnowledgeBookCatagoryData : IDofusData<int>, IComparable<KnowledgeBookCatagoryData>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public LocalizedString Name { get; init; }

    [JsonPropertyName("o")]
    public int Order { get; init; }

    [JsonPropertyName("i")]
    public int Index { get; init; }

    [JsonPropertyName("ep")]
    public int Episode { get; init; }

    [JsonConstructor]
    internal KnowledgeBookCatagoryData()
    {
        Name = LocalizedString.Empty;
    }

    public int CompareTo(KnowledgeBookCatagoryData? other)
    {
        if (other is null)
        {
            return 1;
        }

        return Order.CompareTo(other.Order);
    }
}
