using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data;

public sealed class SpeakingItemData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("m")]
    public string Message { get; init; }

    [JsonPropertyName("s")]
    public int SoundId { get; init; }

    [JsonPropertyName("r")]
    public string ItemsIdCanUse { get; init; }

    [JsonPropertyName("l")]
    public int RequiredLevel { get; init; }

    [JsonPropertyName("p")]
    public double Probability { get; init; }

    [JsonConstructor]
    internal SpeakingItemData()
    {
        Message = string.Empty;
        ItemsIdCanUse = string.Empty;
    }
}

public sealed class SpeakingItemsData : IDofusData
{
    private const string FILE_NAME = "speakingitems.json";

    [JsonPropertyName("SIM")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, SpeakingItemData>))]
    public FrozenDictionary<int, SpeakingItemData> SpeakingItems { get; init; }

    //TODO: SIT in SpeakingItems lang

    [JsonConstructor]
    internal SpeakingItemsData()
    {
        SpeakingItems = FrozenDictionary<int, SpeakingItemData>.Empty;
    }

    internal static SpeakingItemsData Load()
    {
        return Datacenter.LoadDataFromFile<SpeakingItemsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
    }

    public SpeakingItemData? GetSpeakingItemData(int id)
    {
        SpeakingItems.TryGetValue(id, out var speakingItemData);
        return speakingItemData;
    }
}
