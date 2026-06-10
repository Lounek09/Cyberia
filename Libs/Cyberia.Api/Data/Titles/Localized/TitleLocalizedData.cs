using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Titles.Localized;

internal sealed class TitleLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("t")]
    public string Description { get; init; }

    [JsonPropertyName("tf")]
    public string FemaleDescription { get; init; }

    [JsonConstructor]
    internal TitleLocalizedData()
    {
        Description = string.Empty;
        FemaleDescription = string.Empty;
    }
}
