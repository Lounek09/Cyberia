using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Titles.Localized;

internal sealed class TitleLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("t")]
    public string Name { get; init; }

    [JsonConstructor]
    internal TitleLocalizedData()
    {
        Name = string.Empty;
    }
}
