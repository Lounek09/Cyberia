using Cyberia.Api.Data.Pvp.Localized;
using Cyberia.Langzilla.Enums;

using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public IReadOnlyList<IReadOnlyList<PvpGradeData>> PvpGrades { get; init; }

    [JsonConstructor]
    internal PvpRepository()
    {
        HonnorPointThresholds = ReadOnlyCollection<int>.Empty;
        PvpGrades = ReadOnlyCollection<IReadOnlyList<PvpGradeData>>.Empty;
    }

    protected override void LoadLocalizedData(LangType type, Language language)
    {
        var twoLetterISOLanguageName = language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<PvpLocalizedRepository>(type, language);

        if (localizedRepository.PvpGrades.Count == PvpGrades.Count)
        {
            for (var i = 0; i < PvpGrades.Count; i++)
            {
                if (localizedRepository.PvpGrades[i].Count == PvpGrades[i].Count)
                {
                    for (var j = 0; j < PvpGrades[i].Count; j++)
                    {
                        PvpGrades[i][j].Name.TryAdd(twoLetterISOLanguageName, localizedRepository.PvpGrades[i][j].Name);
                        PvpGrades[i][j].ShortName.TryAdd(twoLetterISOLanguageName, localizedRepository.PvpGrades[i][j].ShortName);
                    }
                }
            }
        }
    }
}
