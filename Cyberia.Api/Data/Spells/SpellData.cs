using Cyberia.Api.Data.Breeds;
using Cyberia.Api.Data.Incarnations;
using Cyberia.Api.JsonConverters;
using Cyberia.Api.Values;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Spells;

public sealed class SpellData
    : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("d")]
    public string Description { get; init; }

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
    [JsonConverter(typeof(SpellLevelConverter))]
    public SpellLevelData? SpellLevelData1 { get; init; }

    [JsonPropertyName("l2")]
    [JsonConverter(typeof(SpellLevelConverter))]
    public SpellLevelData? SpellLevelData2 { get; init; }

    [JsonPropertyName("l3")]
    [JsonConverter(typeof(SpellLevelConverter))]
    public SpellLevelData? SpellLevelData3 { get; init; }

    [JsonPropertyName("l4")]
    [JsonConverter(typeof(SpellLevelConverter))]
    public SpellLevelData? SpellLevelData4 { get; init; }

    [JsonPropertyName("l5")]
    [JsonConverter(typeof(SpellLevelConverter))]
    public SpellLevelData? SpellLevelData5 { get; init; }

    [JsonPropertyName("l6")]
    [JsonConverter(typeof(SpellLevelConverter))]
    public SpellLevelData? SpellLevelData6 { get; init; }

    [JsonConstructor]
    internal SpellData()
    {
        Name = string.Empty;
        Description = string.Empty;
        Icon = new();
    }

    public async Task<string> GetImagePath()
    {
        var url = $"{DofusApi.Config.CdnUrl}/images/spells/{Id}.jpg";

        if (await DofusApi.HttpClient.ExistsAsync(url))
        {
            return url;
        }

        return $"{DofusApi.Config.CdnUrl}/images/spells/unknown.png";
    }

    public BreedData? GetBreedData()
    {
        return DofusApi.Datacenter.BreedsData.GetBreedDataById(BreedId) ??
            DofusApi.Datacenter.BreedsData.Breeds.Values.FirstOrDefault(x => x.SpecialSpellId == Id);
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
        foreach (var incarnation in DofusApi.Datacenter.IncarnationsData.Incarnations.Values)
        {
            if (incarnation.SpellsId.Contains(Id))
            {
                return incarnation;
            }
        }

        return null;
    }
}
