using Cyberia.Api.Data.Items;
using Cyberia.Api.Enums;
using Cyberia.Api.Factories.Effects.Elements;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Runes;

public sealed class RuneData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("w")]
    public double Weight { get; init; }

    [JsonPropertyName("ba")]
    public int BaRuneItemId { get; init; }

    [JsonPropertyName("pa")]
    public int? PaRuneItemId { get; init; }

    [JsonPropertyName("ra")]
    public int? RaRuneItemId { get; init; }

    [JsonConstructor]
    internal RuneData() { }

    public ItemData? GetBaRuneItemData()
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemDataById(BaRuneItemId);
    }

    public ItemData? GetPaRuneItemData()
    {
        return PaRuneItemId.HasValue ? DofusApi.Datacenter.ItemsRepository.GetItemDataById(PaRuneItemId.Value) : null;
    }

    public ItemData? GetRaRuneItemData()
    {
        return RaRuneItemId.HasValue ? DofusApi.Datacenter.ItemsRepository.GetItemDataById(RaRuneItemId.Value) : null;
    }

    public int GetPower(RuneType type)
    {
        var itemData = type switch
        {
            RuneType.BA => GetBaRuneItemData(),
            RuneType.PA => GetPaRuneItemData(),
            RuneType.RA => GetRaRuneItemData(),
            _ => null
        };

        if (itemData is null || itemData.GetItemStatsData()?.Effects.FirstOrDefault(x => x is ItemAddEffectEffect) is not ItemAddEffectEffect itemAddEffect)
        {
            return 0;
        }

        return itemAddEffect.Min;
    }
}
