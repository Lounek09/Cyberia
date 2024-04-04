using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Pvp;

public sealed class PvpData
    : IDofusData
{
    private const string FILE_NAME = "pvp.json";
    private static readonly string FILE_PATH = Path.Join(DofusApi.OUTPUT_PATH, FILE_NAME);

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

    internal static async Task<PvpData> LoadAsync()
    {
        return await Datacenter.LoadDataAsync<PvpData>(FILE_PATH);
    }
}
