using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Breeds.Custom;

internal sealed class BreedsCustomData
    : IDofusData
{
    [JsonPropertyName("CG")]
    public IReadOnlyList<BreedCustomData> Breeds { get; init; }

    [JsonConstructor]
    internal BreedsCustomData()
    {
        Breeds = [];
    }
}
