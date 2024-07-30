using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Servers;

public sealed class DefaultServerSpecificTextData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("l")]
    public string Label { get; init; }

    [JsonPropertyName("d")]
    public LocalizedString Description { get; init; }

    [JsonConstructor]
    internal DefaultServerSpecificTextData()
    {
        Label = string.Empty;
        Description = LocalizedString.Empty;
    }
}
