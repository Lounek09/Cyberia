using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Breeds.Localized;

internal sealed class BreedsLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => BreedsRepository.FileName;

    [JsonPropertyName("G")]
    public IReadOnlyList<BreedLocalizedData> Breeds { get; init; }

    [JsonConstructor]
    internal BreedsLocalizedRepository()
    {
        Breeds = ReadOnlyCollection<BreedLocalizedData>.Empty;
    }
}
