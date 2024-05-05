using Cyberia.Api.Factories.Effects;
using Cyberia.Api.Values;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.SpeakingItems;

public sealed class SpeakingItemsTriggerData : IDofusData
{
    [JsonPropertyName("id")]
    public int TriggerId { get; init; }

    [JsonPropertyName("id2")]
    public Corpulence Corpulence { get; init; }

    [JsonPropertyName("v")]
    public IReadOnlyList<int> SpeakingItemsMessageIds { get; init; }

    [JsonConstructor]
    internal SpeakingItemsTriggerData()
    {
        SpeakingItemsMessageIds = Array.Empty<int>();
    }

    public SpeakingItemsMessageData? GetRandomSpeakingItemMessageData()
    {
        var id = SpeakingItemsMessageIds[Random.Shared.Next(SpeakingItemsMessageIds.Count - 1)];
        return DofusApi.Datacenter.SpeakingItemsData.GetSpeakingItemsMessageData(id);
    }
}
