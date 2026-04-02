using Cyberia.Api.Data.Titles.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Primitives;

using System.Collections.Frozen;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Titles;

public sealed class TitlesRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "titles.json";

    [JsonPropertyName("PT")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, TitleData>))]
    public FrozenDictionary<int, TitleData> Titles { get; init; }

    [JsonConstructor]
    internal TitlesRepository()
    {
        Titles = FrozenDictionary<int, TitleData>.Empty;
    }

    public TitleData? GetTitleDataById(int id)
    {
        Titles.TryGetValue(id, out var titleData);
        return titleData;
    }

    public string GetTitleDescriptionById(int id, Language language)
    {
        return GetTitleDescriptionById(id, language.ToCulture());
    }

    public string GetTitleDescriptionById(int id, CultureInfo? culture = null)
    {
        var titleData = GetTitleDataById(id);

        if (titleData is null)
        {
            return Translation.UnknownData(id, culture);
        }

        var description = titleData.Description.ToString(culture);
        var femaleDescription = titleData.FemaleDescription.ToString(culture);

        if (description.Equals(femaleDescription))
        {
            return description;
        }

        return $"{description} / {femaleDescription}";
    }

    protected override void LoadLocalizedData(LangsIdentifier identifier)
    {
        var twoLetterISOLanguageName = identifier.Language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<TitlesLocalizedRepository>(identifier);

        foreach (var titleLocalizedData in localizedRepository.Titles)
        {
            var titleData = GetTitleDataById(titleLocalizedData.Id);
            if (titleData is not null)
            {
                titleData.Description.TryAdd(twoLetterISOLanguageName, titleLocalizedData.Description);
                titleData.FemaleDescription.TryAdd(twoLetterISOLanguageName, titleLocalizedData.FemaleDescription);
            }
        }
    }
}
