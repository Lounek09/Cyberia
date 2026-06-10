using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.SpeakingItems.Localized;

internal sealed class SpeakingItemsMessageLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("m")]
    public string Message { get; init; }

    [JsonConstructor]
    internal SpeakingItemsMessageLocalizedData()
    {
        Message = string.Empty;
    }
}
