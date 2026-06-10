using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Breeds.Custom;

internal sealed class BreedsCustomRepository : DofusCustomRepository, IDofusRepository
{
    public static string FileName => BreedsRepository.FileName;

    [JsonPropertyName("CG")]
    public IReadOnlyList<BreedCustomData> Breeds { get; init; }

    [JsonConstructor]
    internal BreedsCustomRepository()
    {
        Breeds = ReadOnlyCollection<BreedCustomData>.Empty;
    }
}
