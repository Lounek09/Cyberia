using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Values;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Items;

public sealed class ItemTypeData : IDofusData<int>
{
    public static readonly IReadOnlyList<int> NonEnhanceableTypesWeapon = [20, 21, 22, 102, 114];

    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public LocalizedString Name { get; init; }

    [JsonPropertyName("t")]
    public ItemSuperType ItemSuperType { get; init; }

    [JsonPropertyName("z")]
    public EffectArea EffectArea { get; init; }

    [JsonConstructor]
    internal ItemTypeData()
    {
        Name = LocalizedString.Empty;
    }

    public ItemSuperTypeData? GetItemSuperTypeData()
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemSuperTypeDataById((int)ItemSuperType);
    }
}
