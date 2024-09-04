using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Pvp;

public sealed class PvpRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "pvp.json";

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
}
