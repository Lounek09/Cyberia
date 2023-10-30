using Cyberia.Api.Data;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public abstract record QuestObjective(QuestObjectiveData QuestObjectiveData)
    {
        protected Description GetDescription(params object[] parameters)
        {
            string[] strParameters = Array.ConvertAll(parameters, x => x.ToString() ?? string.Empty);

            QuestObjectiveTypeData? questObjectiveTypeData = QuestObjectiveData.GetQuestObjectiveTypeData();
            if (questObjectiveTypeData is not null)
            {
                string coordinate = QuestObjectiveData.GetCoordinate();

                return new(questObjectiveTypeData.Description + (string.IsNullOrEmpty(coordinate) ? "" : $" - {coordinate}"), strParameters);
            }

            Log.Warning("Unknown {QuestObjectiveTypeData} {QuestObjectiveId} ({QuestObjectiveParameters})",
                nameof(QuestObjectiveTypeData),
                QuestObjectiveData.QuestObjectiveTypeId,
                string.Join(", ", strParameters));

            return new(Resources.QuestObjectiveType_Unknown, QuestObjectiveData.QuestObjectiveTypeId.ToString(), string.Join(", ", strParameters));
        }
    }
}
