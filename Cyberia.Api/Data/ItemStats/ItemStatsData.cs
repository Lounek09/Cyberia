using Cyberia.Api.Data.Items;
using Cyberia.Api.Factories.Effects;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.ItemStats;

public sealed class ItemStatsData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    [JsonInclude]
    public EffectReadOnlyCollection Effects { get; internal set; }

    [JsonConstructor]
    internal ItemStatsData()
    {
        Effects = EffectReadOnlyCollection.Empty;
    }

    public ItemData? GetItemData()
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemDataById(Id);
    }
}
