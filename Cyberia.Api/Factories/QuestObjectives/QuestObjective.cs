using Cyberia.Api.Data.Quests;

namespace Cyberia.Api.Factories.QuestObjectives;

public abstract record QuestObjective(QuestObjectiveData QuestObjectiveData)
{
    protected Description GetDescription(params object[] parameters)
    {
        var questObjectiveTypeData = QuestObjectiveData.GetQuestObjectiveTypeData();
        if (questObjectiveTypeData is null)
        {
            var commaSeparatedParameters = string.Join(',', parameters);

            Log.Warning("Unknown {QuestObjectiveTypeData} {QuestObjectiveTypeId} ({QuestObjectiveParameters})",
                nameof(QuestObjectiveTypeData),
                QuestObjectiveData.QuestObjectiveTypeId,
                commaSeparatedParameters);

            return new(Resources.QuestObjectiveType_Unknown, QuestObjectiveData.QuestObjectiveTypeId.ToString(), commaSeparatedParameters);
        }

        var value = questObjectiveTypeData.Description;

        var coordinate = QuestObjectiveData.GetCoordinate();
        if (!string.IsNullOrEmpty(coordinate))
        {
            value += $" - {coordinate}";
        }

        return new(value, Array.ConvertAll(parameters, x => x.ToString() ?? string.Empty));
    }
}
