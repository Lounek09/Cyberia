using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.SpeakingItems;

public sealed class SpeakingItemsMessageData : IDofusData<int>
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
    internal SpeakingItemsMessageData()
    {
        Message = string.Empty;
        ItemsIdCanUse = string.Empty;
    }
}
