using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Pvp.Localized;

internal sealed class PvpLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => PvpRepository.FileName;

    [JsonPropertyName("PP.grds")]
    public IReadOnlyList<IReadOnlyList<PvpGradeData>> PvpGrades { get; init; }

    [JsonConstructor]
    internal PvpLocalizedRepository()
    {
        PvpGrades = ReadOnlyCollection<IReadOnlyList<PvpGradeData>>.Empty;
    }
}
