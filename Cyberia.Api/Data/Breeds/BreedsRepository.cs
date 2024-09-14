using Cyberia.Api.Data.Breeds.Custom;
using Cyberia.Api.Data.Breeds.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Breeds;

public sealed class BreedsRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "classes.json";

    [JsonPropertyName("G")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, BreedData>))]
    public FrozenDictionary<int, BreedData> Breeds { get; init; }

    [JsonConstructor]
    internal BreedsRepository()
    {
        Breeds = FrozenDictionary<int, BreedData>.Empty;
    }

    public BreedData? GetBreedDataById(int id)
    {
        Breeds.TryGetValue(id, out var breedData);
        return breedData;
    }

    public BreedData? GetBreedDataByName(string name)
    {
        name = name.NormalizeToAscii();

        return Breeds.Values.FirstOrDefault(x => StringExtensions.NormalizeToAscii(x.Name).Equals(name));
    }

    public IEnumerable<BreedData> GetBreedsDataByName(string name)
    {
        var names = name.NormalizeToAscii().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return Breeds.Values.Where(x =>
        {
            return names.All(y =>
            {
                return StringExtensions.NormalizeToAscii(x.Name).Contains(y, StringComparison.OrdinalIgnoreCase);
            });
        });
    }

    public string GetBreedNameById(int id)
    {
        var breed = GetBreedDataById(id);

        return breed is null
            ? Translation.Format(ApiTranslations.Unknown_Data, id)
            : breed.Name;
    }

    protected override void LoadCustomData()
    {
        var customRepository = DofusCustomRepository.Load<BreedsCustomRepository>();

        foreach (var breedCustomData in customRepository.Breeds)
        {
            var breedData = GetBreedDataById(breedCustomData.Id);
            if (breedData is not null)
            {
                breedData.SpecialSpellId = breedCustomData.SpecialSpellId;
                breedData.ItemSetId = breedCustomData.ItemSetId;
            }
        }
    }

    protected override void LoadLocalizedData(LangType type, LangLanguage language)
    {
        var twoLetterISOLanguageName = language.ToCulture().TwoLetterISOLanguageName;
        var localizedRepository = DofusLocalizedRepository.Load<BreedsLocalizedRepository>(type, language);

        foreach (var breedLocalizedData in localizedRepository.Breeds)
        {
            var breedData = GetBreedDataById(breedLocalizedData.Id);
            if (breedData is not null)
            {
                breedData.Name.Add(twoLetterISOLanguageName, breedLocalizedData.Name);
                breedData.LongName.Add(twoLetterISOLanguageName, breedLocalizedData.LongName);
                breedData.Description.Add(twoLetterISOLanguageName, breedLocalizedData.Description);
                breedData.ShortDescription.Add(twoLetterISOLanguageName, breedLocalizedData.ShortDescription);
                breedData.TemporisPassiveName.Add(twoLetterISOLanguageName, breedLocalizedData.TemporisPassiveName);
                breedData.TemporisPassiveDescription.Add(twoLetterISOLanguageName, breedLocalizedData.TemporisPassiveDescription);
            }
        }
    }
}
