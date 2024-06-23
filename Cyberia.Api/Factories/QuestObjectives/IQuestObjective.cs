using Cyberia.Api.Data.Quests;

namespace Cyberia.Api.Factories.QuestObjectives;

/// <summary>
/// Represents an objective of a <see cref="QuestStepData"/>.
/// </summary>
public interface IQuestObjective
{
    /// <summary>
    /// Gets the data of the quest objective.
    /// </summary>
    QuestObjectiveData QuestObjectiveData { get; init; }

    /// <summary>
    /// Generates a human-readable description of the quest objective.
    /// </summary>
    /// <returns>The <see cref="Description"/> object containing the description of the quest objective.</returns>
    Description GetDescription();
}
