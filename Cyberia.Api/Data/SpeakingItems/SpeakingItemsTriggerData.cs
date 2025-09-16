using Cyberia.Api.Enums;

using System.Collections.ObjectModel;
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
        SpeakingItemsMessageIds = ReadOnlyCollection<int>.Empty;
    }

    public SpeakingItemsMessageData? GetRandomSpeakingItemMessageData()
    {
        var id = SpeakingItemsMessageIds[Random.Shared.Next(SpeakingItemsMessageIds.Count - 1)];
        return DofusApi.Datacenter.SpeakingItemsRepository.GetSpeakingItemsMessageData(id);
    }
}
