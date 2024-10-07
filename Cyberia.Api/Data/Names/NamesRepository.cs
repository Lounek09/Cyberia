using Cyberia.Api.Data.Names.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Names;

public sealed class NamesRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "names.json";

    [JsonPropertyName("NF.n")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, TaxCollectorLastNameData>))]
    public FrozenDictionary<int, TaxCollectorLastNameData> TaxCollectorLastNames { get; init; }

    [JsonPropertyName("NF.f")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, TaxCollectorFirstNameData>))]
    public FrozenDictionary<int, TaxCollectorFirstNameData> TaxCollectorFirstNames { get; init; }

    [JsonConstructor]
    internal NamesRepository()
    {
        TaxCollectorLastNames = FrozenDictionary<int, TaxCollectorLastNameData>.Empty;
        TaxCollectorFirstNames = FrozenDictionary<int, TaxCollectorFirstNameData>.Empty;
    }

    public TaxCollectorLastNameData? GetTaxCollectorLastNameDataById(int id)
    {
        TaxCollectorLastNames.TryGetValue(id, out var taxCollectorLastNameData);
        return taxCollectorLastNameData;
    }

    public TaxCollectorFirstNameData? GetTaxCollectorFirstNameDataById(int id)
    {
        TaxCollectorFirstNames.TryGetValue(id, out var taxCollectorFirstNameData);
        return taxCollectorFirstNameData;
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

    protected override void LoadLocalizedData(LangType type, LangLanguage language)
    {
        var twoLetterISOLanguageName = language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<NamesLocalizedRepository>(type, language);

        foreach (var taxCollectorLastNameLocalizedData in localizedRepository.TaxCollectorLastNames)
        {
            var taxCollectorLastNameData = GetTaxCollectorLastNameDataById(taxCollectorLastNameLocalizedData.Id);
            taxCollectorLastNameData?.Name.Add(twoLetterISOLanguageName, taxCollectorLastNameLocalizedData.Name);
        }

        foreach (var taxCollectorFirstNameLocalizedData in localizedRepository.TaxCollectorFirstNames)
        {
            var taxCollectorFirstNameData = GetTaxCollectorFirstNameDataById(taxCollectorFirstNameLocalizedData.Id);
            taxCollectorFirstNameData?.Name.Add(twoLetterISOLanguageName, taxCollectorFirstNameLocalizedData.Name);
        }
    }
}
