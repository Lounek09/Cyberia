using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Breeds.Custom;

internal sealed class BreedsCustomRepository : IDofusRepository
{
    [JsonPropertyName("CG")]
    public IReadOnlyList<BreedCustomData> Breeds { get; init; }

    [JsonConstructor]
    internal BreedsCustomRepository()
    {
        Breeds = [];
    }
}
