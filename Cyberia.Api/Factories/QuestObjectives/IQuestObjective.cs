using Cyberia.Api.Data.Quests;
using Cyberia.Langzilla.Primitives;

using System.Globalization;

namespace Cyberia.Api.Factories.QuestObjectives;

/// <summary>
/// Represents an objective of a <see cref="QuestStepData"/>.
/// </summary>
public interface IQuestObjective
{
    /// <summary>
    /// Gets the data of the quest objective.
    /// </summary>
    QuestObjectiveData QuestObjectiveData { get; }

    /// <summary>
    /// Generates a human-readable description of the quest objective for the specified language.
    /// </summary>
    /// <param name="language">The language to generate the description for.</param>
    /// <returns>The <see cref="DescriptionString"/> object containing the description of the quest objective for the specified language.</returns>
    DescriptionString GetDescription(Language language);

    /// <summary>
    /// Generates a human-readable description of the quest objective for the specified culture.
    /// </summary>
    /// <param name="culture">The culture to generate the description for.</param>
    /// <returns>The <see cref="DescriptionString"/> object containing the description of the quest objective for the specified culture.</returns>
    DescriptionString GetDescription(CultureInfo? culture = null);
}
