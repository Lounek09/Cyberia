using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.KnowledgeBook.Localized;

internal sealed class KnowledgeBookArticleLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("a")]
    public string Description { get; init; }

    [JsonPropertyName("k")]
    public List<string> KeyWords { get; init; }

    [JsonConstructor]
    internal KnowledgeBookArticleLocalizedData()
    {
        Name = string.Empty;
        Description = string.Empty;
        KeyWords = [];
    }
}
