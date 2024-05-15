using Cyberia.Api.Factories.Effects;
using Cyberia.Api.JsonConverters;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.ItemStats.Custom;

internal sealed class ItemStatsCustomData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    [JsonConverter(typeof(EffectReadOnlyListConverter))]
    public IReadOnlyList<IEffect> Effects { get; init; }

    [JsonConstructor]
    internal ItemStatsCustomData()
    {
        Effects = [];
    }
}
