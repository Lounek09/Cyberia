using Cyberia.Api.Data.Titles.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
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

    public string GetTitleNameById(int id)
    {
        var titleData = GetTitleDataById(id);

        return titleData is null
            ? Translation.Format(ApiTranslations.Unknown_Data, id)
            : titleData.Name;
    }

    protected override void LoadLocalizedData(LangType type, LangLanguage language)
    {
        var twoLetterISOLanguageName = language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<TitlesLocalizedRepository>(type, language);

        foreach (var titleLocalizedData in localizedRepository.Titles)
        {
            var titleData = GetTitleDataById(titleLocalizedData.Id);
            titleData?.Name.Add(twoLetterISOLanguageName, titleLocalizedData.Name);
        }
    }
}
