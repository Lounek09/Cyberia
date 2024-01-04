using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Breeds.Custom;

internal sealed class BreedCustomData
    : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("bs")]
    public int SpecialSpellId { get; init; }

    [JsonPropertyName("is")]
    public int ItemSetId { get; init; }

    [JsonConstructor]
    internal BreedCustomData()
    {

    }
}

