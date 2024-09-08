using Cyberia.Api.Data.Emotes.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Emotes;

public sealed class EmotesRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "emotes.json";

    [JsonPropertyName("EM")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, EmoteData>))]
    public FrozenDictionary<int, EmoteData> Emotes { get; init; }

    [JsonConstructor]
    internal EmotesRepository()
    {
        Emotes = FrozenDictionary<int, EmoteData>.Empty;
    }

    public EmoteData? GetEmoteDataById(int id)
    {
        Emotes.TryGetValue(id, out var emoteData);
        return emoteData;
    }

    public string GetEmoteNameById(int id)
    {
        var emoteData = GetEmoteDataById(id);

        return emoteData is null
            ? Translation.Format(ApiTranslations.Unknown_Data, id)
            : emoteData.Name;
    }

    protected override void LoadLocalizedData(LangType type, LangLanguage language)
    {
        var twoLetterISOLanguageName = language.ToCulture().TwoLetterISOLanguageName;
        var localizedRepository = DofusLocalizedRepository.Load<EmotesLocalizedRepository>(type, language);

        foreach (var emoteLocalizedData in localizedRepository.Emotes)
        {
            var emoteData = GetEmoteDataById(emoteLocalizedData.Id);
            emoteData?.Name.Add(twoLetterISOLanguageName, emoteLocalizedData.Name);
        }
    }
}
