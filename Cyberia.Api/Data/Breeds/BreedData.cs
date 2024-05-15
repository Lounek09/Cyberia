using Cyberia.Api.Data.ItemSets;
using Cyberia.Api.Data.Spells;
using Cyberia.Api.Managers;
using Cyberia.Api.Values;
using Cyberia.Langzilla.Enums;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Breeds;

public sealed class BreedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("sn")]
    public string Name { get; init; }

    [JsonPropertyName("ln")]
    public string LongName { get; init; }

    [JsonPropertyName("ep")]
    public int Episode { get; init; }

    [JsonPropertyName("d")]
    public string Description { get; init; }

    [JsonPropertyName("sd")]
    public string ShortDescription { get; init; }

    [JsonPropertyName("di")]
    public bool Diabolical { get; init; }

    [JsonPropertyName("s")]
    public IReadOnlyList<int> SpellsId { get; init; }

    [JsonPropertyName("pt")]
    public string TemporisPassiveName { get; init; }

    [JsonPropertyName("pd")]
    public string TemporisPassiveDescription { get; init; }

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

    [JsonConstructor]
    internal BreedData()
    {
        Name = string.Empty;
        LongName = string.Empty;
        Description = string.Empty;
        ShortDescription = string.Empty;
        SpellsId = [];
        TemporisPassiveName = string.Empty;
        TemporisPassiveDescription = string.Empty;
        CloseCombatInfos = new();
        StrengthBoostCost = [];
        VitalityBoostCost = [];
        WisdomBoostCost = [];
        ChanceBoostCost = [];
        AgilityBoostCost = [];
        IntelligenceBoostCost = [];
    }

    public async Task<string> GetIconImagePathAsync(CdnImageSize size)
    {
        return await CdnManager.GetImagePathAsync("artworks/breeds", Id, size);
    }

    public async Task<string> GetBackImagePathAsync(CdnImageSize size)
    {
        return await CdnManager.GetImagePathAsync("artworks/breeds/back", Id, size);
    }

    public async Task<string> GetSlideImagePathAsync(CdnImageSize size)
    {
        return await CdnManager.GetImagePathAsync("artworks/breeds/slide", Id, size);
    }

    public async Task<string> GetWeaponsPreferenceImagePathAsync()
    {
        return await CdnManager.GetImagePathAsync("artworks/breeds/weapons_preference", Id);
    }

    public async Task<string> GetSymbolsImagePathAsync(CdnImageSize size)
    {
        return await CdnManager.GetImagePathAsync("artworks/symbols", Id, size);
    }

    public async Task<string> GetBigImagePathAsync(Gender gender, CdnImageSize size)
    {
        var id = int.Parse($"{Id}{(int)gender}");
        return await CdnManager.GetImagePathAsync("artworks/big", id, size);
    }

    public async Task<string> GetFaceImagePathAsync(Gender gender, CdnImageSize size)
    {
        var id = int.Parse($"{Id}{(int)gender}");
        return await CdnManager.GetImagePathAsync("artworks/faces", id, size);
    }

    public async Task<string> GetMiniImagePathAsync(Gender gender, CdnImageSize size)
    {
        var id = int.Parse($"{Id}{(int)gender}");
        return await CdnManager.GetImagePathAsync("artworks/mini", id, size);
    }

    public IEnumerable<SpellData> GetSpellsData()
    {
        foreach (var spellId in SpellsId)
        {
            var spellData = DofusApi.Datacenter.SpellsRepository.GetSpellDataById(spellId);
            if (spellData is not null &&
                (DofusApi.Config.Type != LangType.Temporis || spellData.SpellCategory is SpellCategory.TemporisBreed))
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
}
