﻿using Cyberia.Api.Data.Crafts;
using Cyberia.Api.Data.ItemSets;
using Cyberia.Api.Data.ItemStats;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects;
using Cyberia.Api.JsonConverters;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Items;

public sealed class ItemData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("nn")]
    public string NormalizedName { get; init; }

    [JsonPropertyName("t")]
    public int ItemTypeId { get; init; }

    [JsonPropertyName("d")]
    public string Description { get; init; }

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
    [JsonConverter(typeof(ItemWeaponDataConverter))]
    public ItemWeaponData? WeaponData { get; init; }

    [JsonPropertyName("c")]
    [JsonConverter(typeof(CriteriaCollectionConverter))]
    public CriteriaCollection Criteria { get; init; }

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
        Name = string.Empty;
        NormalizedName = string.Empty;
        Description = string.Empty;
        Criteria = [];
    }

    public async Task<string> GetImagePath()
    {
        var url = $"{DofusApi.Config.CdnUrl}/images/items/{ItemTypeId}/{GfxId}.png";

        if (await DofusApi.HttpClient.ExistsAsync(url))
        {
            return url;
        }

        return $"{DofusApi.Config.CdnUrl}/images/items/unknown.png";
    }

    public ItemTypeData? GetItemTypeData()
    {
        return DofusApi.Datacenter.ItemsData.GetItemTypeDataById(ItemTypeId);
    }

    public ItemStatsData? GetItemStatsData()
    {
        return DofusApi.Datacenter.ItemsStatsData.GetItemStatDataById(Id);
    }

    public bool IsReallyEnhanceable()
    {
        var itemTypeData = GetItemTypeData();
        if (itemTypeData is not null)
        {
            int[] enhanceableSuperTypes = [1, 2, 3, 4, 5, 10, 11];
            int[] nonEnhanceableWeaponTypes = [20, 21, 22, 102, 114];
            return enhanceableSuperTypes.Contains(itemTypeData.ItemSuperTypeId) && !nonEnhanceableWeaponTypes.Contains(itemTypeData.Id) && Enhanceable;
        }

        return false;
    }

    public ItemSetData? GetItemSetData()
    {
        return DofusApi.Datacenter.ItemSetsData.GetItemSetDataById(ItemSetId);
    }

    public bool IsWeapon()
    {
        return WeaponData is not null;
    }

    public CraftData? GetCraftData()
    {
        return DofusApi.Datacenter.CraftsData.GetCraftDataById(Id);
    }

    public bool Tradeable()
    {
        var itemTypeData = GetItemTypeData();
        var itemStatsData = GetItemStatsData();

        var isQuestItem = itemTypeData is not null && itemTypeData.ItemSuperTypeId == ItemSuperTypeData.SUPER_TYPE_QUEST;
        var isLinkedToAccount = itemStatsData is not null && itemStatsData.Effects.OfType<ExchangeableEffect>().Any(x => x.IsLinkedToAccount());
        var isUnbreakable = itemStatsData is not null && itemStatsData.Effects.OfType<UnbreakableEffect>().Any();

        return !isQuestItem && !Cursed && !isLinkedToAccount && !isUnbreakable;
    }

    public int GetNpcRetailPrice()
    {
        return Price == 0 ? 0 : Math.Max(1, (int)Math.Round(Price / 10d));
    }
}