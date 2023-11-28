using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.KnowledgeBook;

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
