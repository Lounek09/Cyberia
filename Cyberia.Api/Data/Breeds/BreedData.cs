﻿using Cyberia.Api.Data.ItemSets;
using Cyberia.Api.Data.Spells;
using Cyberia.Api.JsonConverters;
using Cyberia.Api.Values;

using System.Collections.ObjectModel;
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
    //TODO: Create a class and a JsonConverter for CloseCombatInfos in BreedData
    internal List<object> CloseCombatInfos { get; init; }

    [JsonPropertyName("b10")]
    [JsonInclude]
    internal List<List<int>> StrengthBoostCost { get; init; }

    [JsonPropertyName("b11")]
    [JsonInclude]
    internal List<List<int>> VitalityBoostCost { get; init; }

    [JsonPropertyName("b12")]
    [JsonInclude]
    internal List<List<int>> WisdomBoostCost { get; init; }

    [JsonPropertyName("b13")]
    [JsonInclude]
    internal List<List<int>> ChanceBoostCost { get; init; }

    [JsonPropertyName("b14")]
    [JsonInclude]
    internal List<List<int>> AgilityBoostCost { get; init; }

    [JsonPropertyName("b15")]
    [JsonInclude]
    internal List<List<int>> IntelligenceBoostCost { get; init; }

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
        CloseCombatInfos = [];
        StrengthBoostCost = [];
        VitalityBoostCost = [];
        WisdomBoostCost = [];
        ChanceBoostCost = [];
        AgilityBoostCost = [];
        IntelligenceBoostCost = [];
    }

    public string GetIconImagePath()
    {
        return $"{DofusApi.Config.CdnUrl}/images/breeds/icons/{Id}.png";
    }

    public string GetPreferenceWeaponsImagePath()
    {
        return $"{DofusApi.Config.CdnUrl}/images/breeds/preference_weapons/weapons_{Id}.png";
    }

    public IEnumerable<SpellData> GetSpellsData()
    {
        foreach (var spellId in SpellsId)
        {
            var spellData = DofusApi.Datacenter.SpellsData.GetSpellDataById(spellId);
            if (spellData is not null && (!DofusApi.Config.Temporis || spellData.SpellCategory is SpellCategory.TemporisBreed))
            {
                yield return spellData;
            }
        }
    }


    //TODO: Re-do this piece of shit
    public string GetCaracteristics()
    {
        var tab = new string[6, 6];

        tab[0, 0] = "Vita";
        tab[0, 1] = "Sasa";
        tab[0, 2] = "Fo  ";
        tab[0, 3] = "Int ";
        tab[0, 4] = "Cha ";
        tab[0, 5] = "Age ";

        foreach (var l in VitalityBoostCost)
        {
            tab[l[1], 0] = "|" + l[0].ToString().PadLeft(4) + " ";
        }

        foreach (var l in WisdomBoostCost)
        {
            tab[l[1], 1] = "|" + l[0].ToString().PadLeft(4) + " ";
        }

        foreach (var l in StrengthBoostCost)
        {
            tab[l[1], 2] = "|" + l[0].ToString().PadLeft(4) + " ";
        }

        foreach (var l in IntelligenceBoostCost)
        {
            tab[l[1], 3] = "|" + l[0].ToString().PadLeft(4) + " ";
        }

        foreach (var l in ChanceBoostCost)
        {
            tab[l[1], 4] = "|" + l[0].ToString().PadLeft(4) + " ";
        }

        foreach (var l in AgilityBoostCost)
        {
            tab[l[1], 5] = "|" + l[0].ToString().PadLeft(4) + " ";
        }

        var value = "`    |1 / " + (Id == 11 ? 2 : 1) + "|2 / 1|3 / 1|4 / 1|5 / 1`\n";

        for (var i = 0; i < 6; i++)
        {
            value += "`";
            for (var j = 0; j < 6; j++)
            {
                if (tab[j, i] is null)
                {
                    value += "|     ";
                }
                else
                {
                    value += tab[j, i];
                }
            }
            value += "`\n";
        }

        return value;
    }

    public SpellData? GetSpecialSpellData()
    {
        return DofusApi.Datacenter.SpellsData.GetSpellDataById(SpecialSpellId);
    }

    public ItemSetData? GetItemSetData()
    {
        return DofusApi.Datacenter.ItemSetsData.GetItemSetDataById(ItemSetId);
    }
}