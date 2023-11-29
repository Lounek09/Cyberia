using Cyberia.Api.JsonConverters;

using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Pvp;

public sealed class PvpData : IDofusData
{
    private const string FILE_NAME = "pvp.json";

    [JsonPropertyName("PP.hp")]
    public IReadOnlyList<int> HonnorPointThresholds { get; init; }

    [JsonPropertyName("PP.maxdp")]
    public int MaxDishonourPoint { get; init; }

    [JsonPropertyName("PP.grds")]
    public IReadOnlyList<IEnumerable<PvpGradeData>> PvpGrades { get; init; }

    [JsonConstructor]
    internal PvpData()
    {
        HonnorPointThresholds = [];
        PvpGrades = [];
    }

    internal static PvpData Load()
    {
        return Datacenter.LoadDataFromFile<PvpData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
    }
}
