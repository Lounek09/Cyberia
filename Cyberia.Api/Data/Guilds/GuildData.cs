﻿using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Guilds;

public sealed class GuildData : IDofusData
{
    //TODO: Properly implement GuildData
    [JsonPropertyName("w")]
    public List<List<int>> BoostCostWeight { get; set; }

    [JsonPropertyName("p")]
    public List<List<int>> BoostCostProspecting { get; set; }

    [JsonPropertyName("c")]
    public List<List<int>> BoostCostTaxCollector { get; set; }

    [JsonPropertyName("x")]
    public List<List<int>> BoostCostWisdom { get; set; }

    [JsonPropertyName("s")]
    public List<List<int>> BoostCostSpell { get; set; }

    [JsonPropertyName("wm")]
    public int WeightMax { get; set; }

    [JsonPropertyName("pm")]
    public int ProspectingMax { get; set; }

    [JsonPropertyName("cm")]
    public int TaxCollectorMax { get; set; }

    [JsonPropertyName("xm")]
    public int WisdomMax { get; set; }

    [JsonPropertyName("sm")]
    public int SpellMax { get; set; }

    [JsonConstructor]
    internal GuildData()
    {
        BoostCostWeight = [];
        BoostCostProspecting = [];
        BoostCostTaxCollector = [];
        BoostCostWisdom = [];
        BoostCostSpell = [];
    }
}