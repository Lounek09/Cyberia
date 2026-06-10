using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Guilds;

public sealed class GuildData : IDofusData
{
    [JsonPropertyName("w")]
    public IReadOnlyList<IReadOnlyList<int>> BoostCostWeight { get; init; }

    [JsonPropertyName("p")]
    public IReadOnlyList<IReadOnlyList<int>> BoostCostProspecting { get; init; }

    [JsonPropertyName("c")]
    public IReadOnlyList<IReadOnlyList<int>> BoostCostTaxCollector { get; init; }

    [JsonPropertyName("x")]
    public IReadOnlyList<IReadOnlyList<int>> BoostCostWisdom { get; init; }

    [JsonPropertyName("s")]
    public IReadOnlyList<IReadOnlyList<int>> BoostCostSpell { get; init; }

    [JsonPropertyName("wm")]
    public int WeightMax { get; init; }

    [JsonPropertyName("pm")]
    public int ProspectingMax { get; init; }

    [JsonPropertyName("cm")]
    public int TaxCollectorMax { get; init; }

    [JsonPropertyName("xm")]
    public int WisdomMax { get; init; }

    [JsonPropertyName("sm")]
    public int SpellMax { get; init; }

    [JsonConstructor]
    internal GuildData()
    {
        BoostCostWeight = ReadOnlyCollection<IReadOnlyList<int>>.Empty;
        BoostCostProspecting = ReadOnlyCollection<IReadOnlyList<int>>.Empty;
        BoostCostTaxCollector = ReadOnlyCollection<IReadOnlyList<int>>.Empty;
        BoostCostWisdom = ReadOnlyCollection<IReadOnlyList<int>>.Empty;
        BoostCostSpell = ReadOnlyCollection<IReadOnlyList<int>>.Empty;
    }
}
