using Cyberia.Api.Factories.Effects;
using Cyberia.Api.JsonConverters;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.ItemStats;

public sealed class ItemStatsData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    [JsonConverter(typeof(ItemEffectListConverter))]
    public IReadOnlyList<IEffect> Effects { get; init; }

    [JsonConstructor]
    internal ItemStatsData()
    {
        Effects = [];
    }
}
