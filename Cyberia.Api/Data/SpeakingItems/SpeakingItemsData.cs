using Cyberia.Api.JsonConverters;
using Cyberia.Api.Values;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.SpeakingItems;

public sealed class SpeakingItemsData : IDofusData
{
    private const string c_fileName = "speakingitems.json";

    private static readonly string s_filePath = Path.Join(DofusApi.OutputPath, c_fileName);

    [JsonPropertyName("SIM")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, SpeakingItemsMessageData>))]
    public FrozenDictionary<int, SpeakingItemsMessageData> SpeakingItemsMessages { get; init; }

    [JsonPropertyName("SIT")]
    public IReadOnlyList<SpeakingItemsTriggerData> SpeakingItemsTriggers { get; init; }


    [JsonConstructor]
    internal SpeakingItemsData()
    {
        SpeakingItemsMessages = FrozenDictionary<int, SpeakingItemsMessageData>.Empty;
        SpeakingItemsTriggers = [];
    }

    internal static async Task<SpeakingItemsData> LoadAsync()
    {
        return await Datacenter.LoadDataAsync<SpeakingItemsData>(s_filePath);
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
