using Cyberia.Api.Factories.Effects;
using Cyberia.Api.JsonConverters;

using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.ItemStats.Custom;

internal sealed class ItemStatsCustomData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    [JsonConverter(typeof(EffectReadOnlyListConverter))]
    public IReadOnlyList<Effect> Effects { get; init; }

    [JsonConstructor]
    internal ItemStatsCustomData()
    {
        Effects = ReadOnlyCollection<Effect>.Empty;
    }
}
