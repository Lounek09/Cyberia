using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.WantedDocument.Localized;

public sealed class WantedDocumentLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("b")]
    public string Body { get; init; }

    [JsonPropertyName("d")]
    public string Description { get; init; }

    [JsonConstructor]
    internal WantedDocumentLocalizedData()
    {
        Body = string.Empty;
        Description = string.Empty;
    }
}
