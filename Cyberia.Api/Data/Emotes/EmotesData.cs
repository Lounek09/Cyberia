using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Emotes;

public sealed class EmotesData
    : IDofusData
{
    private const string FILE_NAME = "emotes.json";
    private static readonly string FILE_PATH = Path.Join(DofusApi.OUTPUT_PATH, FILE_NAME);

    [JsonPropertyName("EM")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, EmoteData>))]
    public FrozenDictionary<int, EmoteData> Emotes { get; init; }

    [JsonConstructor]
    internal EmotesData()
    {
        Emotes = FrozenDictionary<int, EmoteData>.Empty;
    }

    internal static async Task<EmotesData> LoadAsync()
    {
        return await Datacenter.LoadDataAsync<EmotesData>(FILE_PATH);
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
            ? PatternDecoder.Description(Resources.Unknown_Data, id)
            : emoteData.Name;
    }
}
