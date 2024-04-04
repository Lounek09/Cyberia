using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.SpeakingItems;

public sealed class SpeakingItemsData
    : IDofusData
{
    private const string FILE_NAME = "speakingitems.json";
    private static readonly string FILE_PATH = Path.Join(DofusApi.OUTPUT_PATH, FILE_NAME);

    [JsonPropertyName("SIM")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, SpeakingItemData>))]
    public FrozenDictionary<int, SpeakingItemData> SpeakingItems { get; init; }

    //TODO: SIT

    [JsonConstructor]
    internal SpeakingItemsData()
    {
        SpeakingItems = FrozenDictionary<int, SpeakingItemData>.Empty;
    }

    internal static async Task<SpeakingItemsData> LoadAsync()
    {
        return await Datacenter.LoadDataAsync<SpeakingItemsData>(FILE_PATH);
    }

    public SpeakingItemData? GetSpeakingItemData(int id)
    {
        SpeakingItems.TryGetValue(id, out var speakingItemData);
        return speakingItemData;
    }
}
