using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Pvp;

public sealed class PvpData
    : IDofusData
{
    private const string c_fileName = "pvp.json";

    private static readonly string s_filePath = Path.Join(DofusApi.OutputPath, c_fileName);

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
        return await Datacenter.LoadDataAsync<PvpData>(s_filePath);
    }
}
