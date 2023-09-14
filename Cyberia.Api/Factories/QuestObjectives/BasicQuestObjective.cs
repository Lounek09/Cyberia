using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public record BasicQuestObjective(QuestObjective QuestObjectiveData) : IQuestObjective
    {
        public static BasicQuestObjective Create(QuestObjective questObjective)
        {
            return new(questObjective);
        }

        public virtual string GetDescription()
        {
            return GetDescriptionFromParameters(QuestObjectiveData.Parameters.ToArray());
        }

        protected string GetDescriptionFromParameters(params string[] parameters)
        {
            QuestObjectiveType? questObjectiveType = QuestObjectiveData.GetQuestObjectiveType();

            if (questObjectiveType is not null)
            {
                for (int i = 0; i < parameters.Length; i++)
                    parameters[i] = parameters[i].Bold();

                string coordinate = QuestObjectiveData.GetCoordinate();

                return PatternDecoder.DecodeDescription(questObjectiveType.Description, parameters) + (string.IsNullOrEmpty(coordinate) ? "" : $" - {coordinate}");
            }

            DofusApi.Instance.Log.Information("Unknown quest objective {questObjectiveId} (parameters)", QuestObjectiveData.QuestObjectiveTypeId, string.Join(", ", parameters));

            return $"Type d'objectif {QuestObjectiveData.QuestObjectiveTypeId.ToString().Bold()} non référencé ({string.Join(", ", parameters)})";
        }
    }
}
