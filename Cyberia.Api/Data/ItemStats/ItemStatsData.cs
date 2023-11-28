using Cyberia.Api.Factories.Effects;
using Cyberia.Api.JsonConverters;

using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.ItemStats;

public sealed class ItemStatsData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    [JsonConverter(typeof(ItemEffectListConverter))]
    [JsonInclude]
    internal List<IEffect> EffectsCore { get; init; }

    [JsonIgnore]
    public ReadOnlyCollection<IEffect> Effects => EffectsCore.AsReadOnly();

    [JsonConstructor]
    internal ItemStatsData()
    {
        EffectsCore = [];
    }
}
