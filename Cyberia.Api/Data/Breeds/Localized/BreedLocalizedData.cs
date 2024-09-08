using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Breeds.Localized;

internal sealed class BreedLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("sn")]
    public string Name { get; init; }

    [JsonPropertyName("ln")]
    public string LongName { get; init; }

    [JsonPropertyName("d")]
    public string Description { get; init; }

    [JsonPropertyName("sd")]
    public string ShortDescription { get; init; }

    [JsonPropertyName("pt")]
    public string TemporisPassiveName { get; init; }

    [JsonPropertyName("pd")]
    public string TemporisPassiveDescription { get; init; }

    [JsonConstructor]
    internal BreedLocalizedData()
    {
        Name = string.Empty;
        LongName = string.Empty;
        Description = string.Empty;
        ShortDescription = string.Empty;
        TemporisPassiveName = string.Empty;
        TemporisPassiveDescription = string.Empty;
    }
}
