using Cyberia.Api.Data.Breeds.Custom;
using Cyberia.Api.Data.Breeds.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
using System.Globalization;
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

    public BreedData? GetBreedDataBySpecialSpellId(int spellId)
    {
        return Breeds.Values.FirstOrDefault(x => x.SpecialSpellId == spellId);
    }

    public BreedData? GetBreedDataByGladiatroolWeaponItemId(int itemId)
    {
        return Breeds.Values.FirstOrDefault(x => x.GladiatroolWeaponItemId == itemId);
    }

    public BreedData? GetBreedDataByGladiatroolSpellId(int spellId)
    {
        return Breeds.Values.FirstOrDefault(x => x.GladiatroolSpellsId.Contains(spellId));
    }

    public BreedData? GetBreedDataByName(string name, Language language)
    {
        return GetBreedDataByName(name, language.ToCulture());
    }

    public BreedData? GetBreedDataByName(string name, CultureInfo? culture = null)
    {
        name = name.NormalizeToAscii();

        return Breeds.Values.FirstOrDefault(x => x.Name.ToString(culture).NormalizeToAscii().Equals(name));
    }

    public IEnumerable<BreedData> GetBreedsDataByName(string name, Language language)
    {
        return GetBreedsDataByName(name, language.ToCulture());
    }

    public IEnumerable<BreedData> GetBreedsDataByName(string name, CultureInfo? culture = null)
    {
        var names = name.NormalizeToAscii().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return Breeds.Values.Where(x =>
        {
            return names.All(y =>
            {
                return x.Name.ToString(culture).NormalizeToAscii().Contains(y, StringComparison.OrdinalIgnoreCase);
            });
        });
    }

    public string GetBreedNameById(int id, Language language)
    {
        return GetBreedNameById(id, language.ToCulture());
    }

    public string GetBreedNameById(int id, CultureInfo? culture = null)
    {
        var breed = GetBreedDataById(id);

        return breed is null
            ? Translation.UnknownData(id, culture)
            : breed.Name.ToString(culture);
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
                breedData.GladiatroolWeaponItemId = breedCustomData.GladiatroolWeaponItemId;
                breedData.GladiatroolSpellsId = breedCustomData.GladiatroolSpellsId;
            }
        }
    }

    protected override void LoadLocalizedData(LangsIdentifier identifier)
    {
        var twoLetterISOLanguageName = identifier.Language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<BreedsLocalizedRepository>(identifier);

        foreach (var breedLocalizedData in localizedRepository.Breeds)
        {
            var breedData = GetBreedDataById(breedLocalizedData.Id);
            if (breedData is not null)
            {
                breedData.Name.TryAdd(twoLetterISOLanguageName, breedLocalizedData.Name);
                breedData.LongName.TryAdd(twoLetterISOLanguageName, breedLocalizedData.LongName);
                breedData.Description.TryAdd(twoLetterISOLanguageName, breedLocalizedData.Description);
                breedData.ShortDescription.TryAdd(twoLetterISOLanguageName, breedLocalizedData.ShortDescription);
                breedData.TemporisPassiveName.TryAdd(twoLetterISOLanguageName, breedLocalizedData.TemporisPassiveName);
                breedData.TemporisPassiveDescription.TryAdd(twoLetterISOLanguageName, breedLocalizedData.TemporisPassiveDescription);
            }
        }
    }
}
