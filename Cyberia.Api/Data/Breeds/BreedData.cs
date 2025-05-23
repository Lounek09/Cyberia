﻿using Cyberia.Api.Data.Items;
using Cyberia.Api.Data.ItemSets;
using Cyberia.Api.Data.Spells;
using Cyberia.Api.Enums;
using Cyberia.Api.Utils;
using Cyberia.Langzilla.Enums;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Breeds;

public sealed class BreedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("sn")]
    public LocalizedString Name { get; init; }

    [JsonPropertyName("ln")]
    public LocalizedString LongName { get; init; }

    [JsonPropertyName("ep")]
    public int Episode { get; init; }

    [JsonPropertyName("d")]
    public LocalizedString Description { get; init; }

    [JsonPropertyName("sd")]
    public LocalizedString ShortDescription { get; init; }

    [JsonPropertyName("di")]
    public bool Diabolical { get; init; }

    [JsonPropertyName("s")]
    public IReadOnlyList<int> SpellsId { get; init; }

    [JsonPropertyName("pt")]
    public LocalizedString TemporisPassiveName { get; init; }

    [JsonPropertyName("pd")]
    public LocalizedString TemporisPassiveDescription { get; init; }

    [JsonPropertyName("cc")]
    [JsonInclude]
    internal object CloseCombatInfos { get; init; } //Not used anymore

    [JsonPropertyName("b10")]
    [JsonInclude]
    public IReadOnlyList<IReadOnlyList<int>> StrengthBoostCost { get; init; }

    [JsonPropertyName("b11")]
    [JsonInclude]
    public IReadOnlyList<IReadOnlyList<int>> VitalityBoostCost { get; init; }

    [JsonPropertyName("b12")]
    [JsonInclude]
    public IReadOnlyList<IReadOnlyList<int>> WisdomBoostCost { get; init; }

    [JsonPropertyName("b13")]
    [JsonInclude]
    public IReadOnlyList<IReadOnlyList<int>> ChanceBoostCost { get; init; }

    [JsonPropertyName("b14")]
    [JsonInclude]
    public IReadOnlyList<IReadOnlyList<int>> AgilityBoostCost { get; init; }

    [JsonPropertyName("b15")]
    [JsonInclude]
    public IReadOnlyList<IReadOnlyList<int>> IntelligenceBoostCost { get; init; }

    [JsonIgnore]
    public int SpecialSpellId { get; internal set; }

    [JsonIgnore]
    public int ItemSetId { get; internal set; }

    [JsonIgnore]
    public int GladiatroolWeaponItemId { get; internal set; }

    [JsonIgnore]
    public IReadOnlyList<int> GladiatroolSpellsId { get; internal set; }

    [JsonConstructor]
    internal BreedData()
    {
        Name = LocalizedString.Empty;
        LongName = LocalizedString.Empty;
        Description = LocalizedString.Empty;
        ShortDescription = LocalizedString.Empty;
        SpellsId = [];
        TemporisPassiveName = LocalizedString.Empty;
        TemporisPassiveDescription = LocalizedString.Empty;
        CloseCombatInfos = new();
        StrengthBoostCost = [];
        VitalityBoostCost = [];
        WisdomBoostCost = [];
        ChanceBoostCost = [];
        AgilityBoostCost = [];
        IntelligenceBoostCost = [];
        GladiatroolSpellsId = [];
    }

    public async Task<string> GetIconImagePathAsync(CdnImageSize size)
    {
        return await ImageUrlProvider.GetImagePathAsync("artworks/breeds", Id, size);
    }

    public async Task<string> GetBackImagePathAsync(CdnImageSize size)
    {
        return await ImageUrlProvider.GetImagePathAsync("artworks/breeds/back", Id, size);
    }

    public async Task<string> GetSlideImagePathAsync(CdnImageSize size)
    {
        return await ImageUrlProvider.GetImagePathAsync("artworks/breeds/slide", Id, size);
    }

    public async Task<string> GetWeaponsPreferenceImagePathAsync()
    {
        return await ImageUrlProvider.GetImagePathAsync("artworks/breeds/weapons_preference", Id);
    }

    public async Task<string> GetSymbolsImagePathAsync(CdnImageSize size)
    {
        return await ImageUrlProvider.GetImagePathAsync("artworks/symbols", Id, size);
    }

    public async Task<string> GetBigImagePathAsync(Gender gender, CdnImageSize size)
    {
        var id = int.Parse($"{Id}{(int)gender}");
        return await ImageUrlProvider.GetImagePathAsync("artworks/big", id, size);
    }

    public async Task<string> GetFaceImagePathAsync(Gender gender, CdnImageSize size)
    {
        var id = int.Parse($"{Id}{(int)gender}");
        return await ImageUrlProvider.GetImagePathAsync("artworks/faces", id, size);
    }

    public async Task<string> GetMiniImagePathAsync(Gender gender, CdnImageSize size)
    {
        var id = int.Parse($"{Id}{(int)gender}");
        return await ImageUrlProvider.GetImagePathAsync("artworks/mini", id, size);
    }

    public IEnumerable<SpellData> GetSpellsData()
    {
        foreach (var spellId in SpellsId)
        {
            var spellData = DofusApi.Datacenter.SpellsRepository.GetSpellDataById(spellId);
            if (spellData is not null && (DofusApi.Config.Type != LangType.Temporis || spellData.SpellCategory is SpellCategory.Temporis3Breed))
            {
                yield return spellData;
            }
        }
    }

    public SpellData? GetSpecialSpellData()
    {
        return DofusApi.Datacenter.SpellsRepository.GetSpellDataById(SpecialSpellId);
    }

    public ItemSetData? GetItemSetData()
    {
        return DofusApi.Datacenter.ItemSetsRepository.GetItemSetDataById(ItemSetId);
    }

    public ItemData? GetGladiatroolWeaponItemData()
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemDataById(GladiatroolWeaponItemId);
    }

    public IEnumerable<SpellData> GetGladiatroolSpellsData()
    {
        foreach (var spellId in GladiatroolSpellsId)
        {
            var spellData = DofusApi.Datacenter.SpellsRepository.GetSpellDataById(spellId);
            if (spellData is not null)
            {
                yield return spellData;
            }
        }
    }
}
