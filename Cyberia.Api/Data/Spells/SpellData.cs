using Cyberia.Api.Data.Breeds;
using Cyberia.Api.Data.Incarnations;
using Cyberia.Api.Managers;
using Cyberia.Api.Values;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Spells;

public sealed class SpellData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public LocalizedString Name { get; init; }

    [JsonPropertyName("d")]
    public LocalizedString Description { get; init; }

    [JsonPropertyName("i")]
    public SpellIconData Icon { get; init; }

    [JsonPropertyName("p")]
    public bool Passive { get; init; }

    [JsonPropertyName("g")]
    public bool GlobalInterval { get; init; }

    [JsonPropertyName("b")]
    public int BreedId { get; init; }

    [JsonPropertyName("t")]
    public SpellType SpellType { get; init; }

    [JsonPropertyName("o")]
    public SpellOrigin SpellOrigin { get; init; }

    [JsonPropertyName("c")]
    public SpellCategory SpellCategory { get; init; }

    [JsonPropertyName("l1")]
    public SpellLevelData? SpellLevelData1 { get; init; }

    [JsonPropertyName("l2")]
    public SpellLevelData? SpellLevelData2 { get; init; }

    [JsonPropertyName("l3")]
    public SpellLevelData? SpellLevelData3 { get; init; }

    [JsonPropertyName("l4")]
    public SpellLevelData? SpellLevelData4 { get; init; }

    [JsonPropertyName("l5")]
    public SpellLevelData? SpellLevelData5 { get; init; }

    [JsonPropertyName("l6")]
    public SpellLevelData? SpellLevelData6 { get; init; }

    [JsonConstructor]
    internal SpellData()
    {
        Name = LocalizedString.Empty;
        Description = LocalizedString.Empty;
        Icon = new();
    }

    public async Task<string> GetIconImagePathAsync(CdnImageSize size)
    {
        return await CdnManager.GetImagePathAsync("spells", Id, size, "jpg");
    }

    public BreedData? GetBreedData()
    {
        return DofusApi.Datacenter.BreedsRepository.GetBreedDataById(BreedId) ??
            DofusApi.Datacenter.BreedsRepository.Breeds.Values.FirstOrDefault(x => x.SpecialSpellId == Id);
    }

    public SpellLevelData? GetSpellLevelData(int level = 1)
    {
        return level switch
        {
            1 => SpellLevelData1,
            2 => SpellLevelData2,
            3 => SpellLevelData3,
            4 => SpellLevelData4,
            5 => SpellLevelData5,
            6 => SpellLevelData6,
            _ => null,
        };
    }

    public IEnumerable<SpellLevelData> GetSpellLevelsData()
    {
        for (var i = 1; i <= 6; i++)
        {
            var spellLevelData = GetSpellLevelData(i);
            if (spellLevelData is not null)
            {
                yield return spellLevelData;
            }
        }
    }

    public int GetMaxLevelNumber()
    {
        for (var i = 6; i > 0; i--)
        {
            if (GetSpellLevelData(i) is not null)
            {
                return i;
            }
        }

        return -1;
    }

    public int GetNeededLevel()
    {
        return SpellLevelData1 is null ? -1 : SpellLevelData1.NeededLevel;
    }

    public IncarnationData? GetIncarnationData()
    {
        foreach (var incarnation in DofusApi.Datacenter.IncarnationsRepository.Incarnations.Values)
        {
            if (incarnation.SpellsId.Contains(Id))
            {
                return incarnation;
            }
        }

        return null;
    }
}
