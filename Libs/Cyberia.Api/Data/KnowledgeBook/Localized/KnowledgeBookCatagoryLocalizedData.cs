using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.KnowledgeBook.Localized;

internal sealed class KnowledgeBookCatagoryLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonConstructor]
    internal KnowledgeBookCatagoryLocalizedData()
    {
        Name = string.Empty;
    }
}
