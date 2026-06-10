using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Quests.Localized;

internal sealed class QuestsLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => QuestsRepository.FileName;

    [JsonPropertyName("Q.q")]
    public IReadOnlyList<QuestLocalizedData> Quests { get; init; }

    [JsonPropertyName("Q.s")]
    public IReadOnlyList<QuestStepLocalizedData> QuestSteps { get; init; }

    [JsonPropertyName("Q.o")]
    public IReadOnlyList<QuestObjectiveLocalizedData> QuestObjectives { get; init; }

    [JsonPropertyName("Q.t")]
    public IReadOnlyList<QuestObjectiveTypeLocalizedData> QuestObjectiveTypes { get; init; }

    [JsonConstructor]
    internal QuestsLocalizedRepository()
    {
        Quests = ReadOnlyCollection<QuestLocalizedData>.Empty;
        QuestSteps = ReadOnlyCollection<QuestStepLocalizedData>.Empty;
        QuestObjectives = ReadOnlyCollection<QuestObjectiveLocalizedData>.Empty;
        QuestObjectiveTypes = ReadOnlyCollection<QuestObjectiveTypeLocalizedData>.Empty;
    }
}
