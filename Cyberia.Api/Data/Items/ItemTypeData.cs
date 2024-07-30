using Cyberia.Api.Factories.EffectAreas;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Items;

public sealed class ItemTypeData : IDofusData<int>
{
    public const int Pet = 18;

    public static readonly IReadOnlyList<int> NonEnhanceableTypesWeapon = [20, 21, 22, 102, 114];

    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public LocalizedString Name { get; init; }

    [JsonPropertyName("t")]
    public int ItemSuperTypeId { get; init; }

    [JsonPropertyName("z")]
    public EffectArea EffectArea { get; init; }

    [JsonConstructor]
    internal ItemTypeData()
    {
        Name = LocalizedString.Empty;
    }
}
