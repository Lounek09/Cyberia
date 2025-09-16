using Cyberia.Api.Data.Items;
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
    [JsonConverter(typeof(EffectReadOnlyListConverter))]
    [JsonInclude]
    public IReadOnlyList<IEffect> Effects { get; internal set; }

    [JsonConstructor]
    internal ItemStatsData()
    {
        Effects = ReadOnlyCollection<IEffect>.Empty;
    }

    public ItemData? GetItemData()
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemDataById(Id);
    }
}
