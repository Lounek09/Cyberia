using Cyberia.Api.Data.Emotes.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Primitives;

using System.Collections.Frozen;
using System.Globalization;
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

    public string GetEmoteNameById(int id, Language language)
    {
        return GetEmoteNameById(id, language.ToCulture());
    }

    public string GetEmoteNameById(int id, CultureInfo? culture = null)
    {
        var emoteData = GetEmoteDataById(id);

        return emoteData is null
            ? Translation.UnknownData(id, culture)
            : emoteData.Name.ToString(culture);
    }

    protected override void LoadLocalizedData(LangsIdentifier identifier)
    {
        var twoLetterISOLanguageName = identifier.Language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<EmotesLocalizedRepository>(identifier);

        foreach (var emoteLocalizedData in localizedRepository.Emotes)
        {
            var emoteData = GetEmoteDataById(emoteLocalizedData.Id);
            emoteData?.Name.TryAdd(twoLetterISOLanguageName, emoteLocalizedData.Name);
        }
    }
}
