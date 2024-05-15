using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Names;

public sealed class TaxCollectorNamesRepository : IDofusRepository
{
    private const string c_fileName = "names.json";

    [JsonPropertyName("NF.n")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, TaxCollectorLastNameData>))]
    public FrozenDictionary<int, TaxCollectorLastNameData> TaxCollectorLastNames { get; init; }

    [JsonPropertyName("NF.f")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, TaxCollectorFirstNameData>))]
    public FrozenDictionary<int, TaxCollectorFirstNameData> TaxCollectorFirstNames { get; init; }

    [JsonConstructor]
    internal TaxCollectorNamesRepository()
    {
        TaxCollectorLastNames = FrozenDictionary<int, TaxCollectorLastNameData>.Empty;
        TaxCollectorFirstNames = FrozenDictionary<int, TaxCollectorFirstNameData>.Empty;
    }

    internal static TaxCollectorNamesRepository Load(string directoryPath)
    {
        var filePath = Path.Join(directoryPath, c_fileName);

        return Datacenter.LoadRepository<TaxCollectorNamesRepository>(filePath);
    }

    public string GetRandomTaxCollectorName()
    {
        if (TaxCollectorLastNames.Count > 0 && TaxCollectorFirstNames.Count > 0)
        {
            var lastNameIndex = Random.Shared.Next(0, TaxCollectorLastNames.Count - 1);
            var firstNameIndex = Random.Shared.Next(0, TaxCollectorFirstNames.Count - 1);

            return (TaxCollectorFirstNames[firstNameIndex].Name + " " + TaxCollectorLastNames[lastNameIndex].Name).Replace("[wip] ", string.Empty);
        }

        return string.Empty;
    }
}
