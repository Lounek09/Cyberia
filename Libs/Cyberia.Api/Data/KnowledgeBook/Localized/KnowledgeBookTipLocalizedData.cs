using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.KnowledgeBook.Localized;

internal sealed class KnowledgeBookTipLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("c")]
    public string Description { get; init; }

    [JsonConstructor]
    internal KnowledgeBookTipLocalizedData()
    {
        Description = string.Empty;
    }
}
