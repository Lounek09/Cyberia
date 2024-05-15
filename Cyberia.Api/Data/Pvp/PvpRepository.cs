using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Pvp;

public sealed class PvpRepository : IDofusRepository
{
    private const string c_fileName = "pvp.json";

    [JsonPropertyName("PP.hp")]
    public IReadOnlyList<int> HonnorPointThresholds { get; init; }

    [JsonPropertyName("PP.maxdp")]
    public int MaxDishonourPoint { get; init; }

    [JsonPropertyName("PP.grds")]
    public IReadOnlyList<IEnumerable<PvpGradeData>> PvpGrades { get; init; }

    [JsonConstructor]
    internal PvpRepository()
    {
        HonnorPointThresholds = [];
        PvpGrades = [];
    }

    internal static PvpRepository Load(string directoryPath)
    {
        var filePath = Path.Join(directoryPath, c_fileName);

        return Datacenter.LoadRepository<PvpRepository>(filePath);
    }
}
