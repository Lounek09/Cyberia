using Cyberia.Api.Enums;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Items;

public sealed class ItemSuperTypeData : IDofusData<int>
{
    public static readonly IReadOnlyList<ItemSuperType> EnhanceableSuperTypes =
    [
        ItemSuperType.Amulet,
        ItemSuperType.Weapon,
        ItemSuperType.Ring,
        ItemSuperType.Belt,
        ItemSuperType.Boots,
        ItemSuperType.Hat,
        ItemSuperType.Back
    ];

    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    [JsonInclude]
    internal bool V { get; init; }

    [JsonIgnore]
    public IReadOnlyList<int> SlotsId { get; internal set; }

    [JsonConstructor]
    internal ItemSuperTypeData()
    {
        SlotsId = [];
    }
}
