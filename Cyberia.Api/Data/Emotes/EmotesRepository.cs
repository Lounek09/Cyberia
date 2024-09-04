using Cyberia.Api.JsonConverters;

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

    public EmoteData? GetEmoteById(int id)
    {
        Emotes.TryGetValue(id, out var emoteData);
        return emoteData;
    }

    public string GetEmoteNameById(int id)
    {
        var emoteData = GetEmoteById(id);

        return emoteData is null
            ? Translation.Format(ApiTranslations.Unknown_Data, id)
            : emoteData.Name;
    }
}
