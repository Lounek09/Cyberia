using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data;

public sealed class TaxCollectorLastNameData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public string Name { get; init; }

    [JsonConstructor]
    internal TaxCollectorLastNameData()
    {
        Name = string.Empty;
    }
}

public sealed class TaxCollectorFirstNameData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public string Name { get; init; }

    [JsonConstructor]
    internal TaxCollectorFirstNameData()
    {
        Name = string.Empty;
    }
}

public sealed class TaxCollectorNamesData : IDofusData
{
    private const string FILE_NAME = "names.json";

    [JsonPropertyName("NF.n")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, TaxCollectorLastNameData>))]
    public FrozenDictionary<int, TaxCollectorLastNameData> TaxCollectorLastNames { get; init; }

    [JsonPropertyName("NF.f")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, TaxCollectorFirstNameData>))]
    public FrozenDictionary<int, TaxCollectorFirstNameData> TaxCollectorFirstNames { get; init; }

    [JsonConstructor]
    internal TaxCollectorNamesData()
    {
        TaxCollectorLastNames = FrozenDictionary<int, TaxCollectorLastNameData>.Empty;
        TaxCollectorFirstNames = FrozenDictionary<int, TaxCollectorFirstNameData>.Empty;
    }

    internal static TaxCollectorNamesData Load()
    {
        return Datacenter.LoadDataFromFile<TaxCollectorNamesData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
    }

    public string GetRandomTaxCollectorName()
    {
        if (TaxCollectorLastNames.Count > 0 && TaxCollectorFirstNames.Count > 0)
        {
            var lastNameIndex = Random.Shared.Next(0, TaxCollectorLastNames.Count - 1);
            var firstNameIndex = Random.Shared.Next(0, TaxCollectorFirstNames.Count - 1);

            return (TaxCollectorFirstNames[firstNameIndex].Name + " " + TaxCollectorLastNames[lastNameIndex].Name).Replace("[wip] ", "");
        }

        return string.Empty;
    }
}
