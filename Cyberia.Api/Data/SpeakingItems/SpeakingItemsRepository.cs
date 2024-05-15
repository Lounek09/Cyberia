using Cyberia.Api.JsonConverters;
using Cyberia.Api.Values;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.SpeakingItems;

public sealed class SpeakingItemsRepository : IDofusRepository
{
    private const string c_fileName = "speakingitems.json";

    [JsonPropertyName("SIM")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, SpeakingItemsMessageData>))]
    public FrozenDictionary<int, SpeakingItemsMessageData> SpeakingItemsMessages { get; init; }

    [JsonPropertyName("SIT")]
    public IReadOnlyList<SpeakingItemsTriggerData> SpeakingItemsTriggers { get; init; }


    [JsonConstructor]
    internal SpeakingItemsRepository()
    {
        SpeakingItemsMessages = FrozenDictionary<int, SpeakingItemsMessageData>.Empty;
        SpeakingItemsTriggers = [];
    }

    internal static SpeakingItemsRepository Load(string directoryPath)
    {
        var filePath = Path.Join(directoryPath, c_fileName);

        return Datacenter.LoadRepository<SpeakingItemsRepository>(filePath);
    }

    public SpeakingItemsMessageData? GetSpeakingItemsMessageData(int id)
    {
        SpeakingItemsMessages.TryGetValue(id, out var speakingItemData);
        return speakingItemData;
    }

    public SpeakingItemsTriggerData? GetSpeakingItemsTriggerData(int triggerId, Corpulence corpulence)
    {
        return SpeakingItemsTriggers.FirstOrDefault(x => x.TriggerId == triggerId && x.Corpulence == corpulence);
    }
}
