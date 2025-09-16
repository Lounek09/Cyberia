using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Names.Localized;

internal sealed class NamesLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => NamesRepository.FileName;

    [JsonPropertyName("NF.n")]
    public IReadOnlyList<TaxCollectorLastNameLocalizedData> TaxCollectorLastNames { get; init; }

    [JsonPropertyName("NF.f")]
    public IReadOnlyList<TaxCollectorFirstNameLocalizedData> TaxCollectorFirstNames { get; init; }

    [JsonConstructor]
    internal NamesLocalizedRepository()
    {
        TaxCollectorLastNames = ReadOnlyCollection<TaxCollectorLastNameLocalizedData>.Empty;
        TaxCollectorFirstNames = ReadOnlyCollection<TaxCollectorFirstNameLocalizedData>.Empty;
    }
}
