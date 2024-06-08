using Cyberia.Api.Data.Quests;

using System.Text;

namespace Cyberia.Api.Factories.QuestObjectives;

public abstract record QuestObjective(QuestObjectiveData QuestObjectiveData)
{
    protected Description GetDescription<T>(T parameter)
    {
        return GetDescription(parameter?.ToString() ?? string.Empty);
    }

    protected Description GetDescription<T0, T1>(T0 parameter0, T1 parameter1)
    {
        return GetDescription(
            parameter0?.ToString() ?? string.Empty,
            parameter1?.ToString() ?? string.Empty);
    }

    protected Description GetDescription<T0, T1, T2>(T0 parameter0, T1 parameter1, T2 parameter2)
    {
        return GetDescription(
            parameter0?.ToString() ?? string.Empty,
            parameter1?.ToString() ?? string.Empty,
            parameter2?.ToString() ?? string.Empty);
    }

    protected Description GetDescription(params string[] parameters)
    {
        var questObjectiveTypeData = QuestObjectiveData.GetQuestObjectiveTypeData();
        if (questObjectiveTypeData is null)
        {
            Log.Warning("Unknown QuestObjectiveTypeData {@QuestObjective}", this);

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
