using Cyberia.Api.Data.Crafts;
using Cyberia.Api.Data.ItemSets;
using Cyberia.Api.Data.ItemStats;
using Cyberia.Api.Data.Npcs;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects;
using Cyberia.Api.Managers;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Items;

public sealed class ItemData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public LocalizedString Name { get; init; }

    [JsonPropertyName("nn")]
    public LocalizedString NormalizedName { get; init; }

    [JsonPropertyName("t")]
    public int ItemTypeId { get; init; }

    [JsonPropertyName("d")]
    public LocalizedString Description { get; init; }

    [JsonPropertyName("ep")]
    public int Episode { get; init; }

    [JsonPropertyName("g")]
    public int GfxId { get; init; }

    [JsonPropertyName("l")]
    public int Level { get; init; }

    [JsonPropertyName("wd")]
    public bool Wieldable { get; init; }

    [JsonPropertyName("fm")]
    public bool Enhanceable { get; init; }

    [JsonPropertyName("w")]
    public int Weight { get; init; }

    [JsonPropertyName("et")]
    public bool Ethereal { get; init; }

    [JsonPropertyName("an")]
    public int AnimationId { get; init; }

    [JsonPropertyName("tw")]
    public bool TwoHanded { get; init; }

    [JsonPropertyName("e")]
    public ItemWeaponData? WeaponData { get; init; }

    [JsonPropertyName("c")]
    public CriteriaReadOnlyCollection Criteria { get; init; }

    [JsonPropertyName("s")]
    public int ItemSetId { get; init; }

    [JsonPropertyName("u")]
    public bool Usable { get; init; }

    [JsonPropertyName("ut")]
    public bool Targetable { get; init; }

    [JsonPropertyName("m")]
    public bool Cursed { get; init; }

    [JsonPropertyName("ce")]
    public bool Ceremonial { get; init; }

    [JsonPropertyName("p")]
    public int Price { get; init; }

    [JsonConstructor]
    internal ItemData()
    {
        Name = LocalizedString.Empty;
        NormalizedName = LocalizedString.Empty;
        Description = LocalizedString.Empty;
        Criteria = [];
    }

    public async Task<string> GetImagePathAsync(CdnImageSize size)
    {
        return await CdnManager.GetImagePathAsync($"items/{ItemTypeId}", GfxId, size);
    }

    public ItemTypeData? GetItemTypeData()
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemTypeDataById(ItemTypeId);
    }

    public ItemStatsData? GetItemStatsData()
    {
        return DofusApi.Datacenter.ItemsStatsRepository.GetItemStatDataById(Id);
    }

    public ItemSetData? GetItemSetData()
    {
        return DofusApi.Datacenter.ItemSetsRepository.GetItemSetDataById(ItemSetId);
    }

    public CraftData? GetCraftData()
    {
        return DofusApi.Datacenter.CraftsRepository.GetCraftDataById(Id);
    }

    public bool IsWeapon()
    {
        return WeaponData is not null;
    }

    public bool IsQuestItem()
    {
        return GetItemTypeData()?.ItemSuperTypeId == ItemSuperTypeData.Quest;
    }

    public bool IsReallyEnhanceable()
    {
        var itemTypeData = GetItemTypeData();
        if (itemTypeData is not null)
        {
            return !Ceremonial &&
                Enhanceable &&
                ItemSuperTypeData.EnhanceableSuperTypes.Contains(itemTypeData.ItemSuperTypeId) &&
                !ItemTypeData.NonEnhanceableTypesWeapon.Contains(itemTypeData.Id);
        }

        return false;
    }

    public bool Tradeable()
    {
        var hasEffectPreventingTrade = GetItemStatsData()?.Effects.Any(x =>
            (x is MarkNotTradableEffect markNotTradableEffect && markNotTradableEffect.IsLinkedToAccount()) ||
            x is MarkNeverTradableStrongEffect ||
            x is LockToAccountEffect ||
            (x is LockToAccountUntilEffect lockToAccountUntilEffect && lockToAccountUntilEffect.DateTime > DateTime.Now) ||
            x is ItemUnbreakableEffect) ?? false;

        return !IsQuestItem() && !Cursed && !hasEffectPreventingTrade;
    }

    public int GetNpcRetailPrice()
    {
        return Price == 0 ? 0 : Math.Max(1, (int)Math.Round(Price * NpcData.RetailRatio));
    }
}
