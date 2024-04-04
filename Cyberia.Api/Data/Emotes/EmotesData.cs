using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Emotes;

public sealed class EmotesData
    : IDofusData
{
    private const string c_fileName = "emotes.json";

    private static readonly string s_filePath = Path.Join(DofusApi.OutputPath, c_fileName);

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
        return await Datacenter.LoadDataAsync<EmotesData>(s_filePath);
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
