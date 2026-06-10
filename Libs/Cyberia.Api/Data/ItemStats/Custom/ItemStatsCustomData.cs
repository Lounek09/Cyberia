using Cyberia.Api.Factories.Effects;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.ItemStats.Custom;

internal sealed class ItemStatsCustomData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public EffectReadOnlyCollection Effects { get; init; }

    [JsonConstructor]
    internal ItemStatsCustomData()
    {
        Effects = EffectReadOnlyCollection.Empty;
    }
}
