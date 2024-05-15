using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Emotes;

public sealed class EmotesRepository : IDofusRepository
{
    private const string c_fileName = "emotes.json";

    [JsonPropertyName("EM")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, EmoteData>))]
    public FrozenDictionary<int, EmoteData> Emotes { get; init; }

    [JsonConstructor]
    internal EmotesRepository()
    {
        Emotes = FrozenDictionary<int, EmoteData>.Empty;
    }

    internal static EmotesRepository Load(string directoryPath)
    {
        var filePath = Path.Join(directoryPath, c_fileName);

        return Datacenter.LoadRepository<EmotesRepository>(filePath);
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
