using Cyberia.Api.Data.Quests;

using System.Text;

namespace Cyberia.Api.Factories.QuestObjectives;

/// <inheritdoc cref="IQuestObjective"/>
public abstract record QuestObjective : IQuestObjective
{
    public QuestObjectiveData QuestObjectiveData { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="QuestObjective"/> record.
    /// </summary>
    /// <param name="questObjectiveData">The data of the quest objective.</param>
    protected QuestObjective(QuestObjectiveData questObjectiveData)
    {
        QuestObjectiveData = questObjectiveData;
    }

    public abstract DescriptionString GetDescription();

    /// <inheritdoc cref="IQuestObjective.GetDescription"/>
    protected DescriptionString GetDescription<T>(T parameter)
    {
        return GetDescription(parameter?.ToString() ?? string.Empty);
    }

    /// <inheritdoc cref="IQuestObjective.GetDescription"/>
    protected DescriptionString GetDescription<T0, T1>(T0 parameter0, T1 parameter1)
    {
        return GetDescription(
            parameter0?.ToString() ?? string.Empty,
            parameter1?.ToString() ?? string.Empty);
    }

    /// <inheritdoc cref="IQuestObjective.GetDescription"/>
    protected DescriptionString GetDescription<T0, T1, T2>(T0 parameter0, T1 parameter1, T2 parameter2)
    {
        return GetDescription(
            parameter0?.ToString() ?? string.Empty,
            parameter1?.ToString() ?? string.Empty,
            parameter2?.ToString() ?? string.Empty);
    }

    /// <inheritdoc cref="IQuestObjective.GetDescription"/>
    protected DescriptionString GetDescription(params string[] parameters)
    {
        var questObjectiveTypeData = QuestObjectiveData.GetQuestObjectiveTypeData();
        if (questObjectiveTypeData is null)
        {
            if (this is not UntranslatedQuestObjective)
            {
                Log.Warning("Unknown QuestObjectiveTypeData {@QuestObjective}", this);
            }

            return new(ApiTranslations.QuestObjectiveType_Unknown,
            QuestObjectiveData.QuestObjectiveTypeId.ToString(),
            string.Join(',', QuestObjectiveData.Parameters));
        }

        StringBuilder builder = new(questObjectiveTypeData.Description);

        var coordinate = QuestObjectiveData.GetCoordinate();
        if (!string.IsNullOrEmpty(coordinate))
        {
            builder.Append(" - ");
            builder.Append(coordinate);
        }

        return new(builder.ToString(), parameters);
    }
}
