using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public record BasicQuestObjective(QuestObjective QuestObjective) : IQuestObjective
    {
        public static BasicQuestObjective Create(QuestObjective questObjective)
        {
            return new(questObjective);
        }

        public virtual string GetDescription()
        {
            return GetDescriptionFromParameters(QuestObjective.Parameters.ToArray());
        }

        protected string GetDescriptionFromParameters(params string[] parameters)
        {
            QuestObjectiveType? questObjectiveType = QuestObjective.GetQuestObjectiveType();

            if (questObjectiveType is not null)
            {
                for (int i = 0; i < parameters.Length; i++)
                    parameters[i] = parameters[i].Bold();

                string coordinate = QuestObjective.GetCoordinate();

                return PatternDecoder.DecodeDescription(questObjectiveType.Description, parameters) + (string.IsNullOrEmpty(coordinate) ? "" : $" - {coordinate}");
            }

            return $"Type d'objectif {QuestObjective.QuestObjectiveTypeId.ToString().Bold()} non référencé ({string.Join(", ", parameters)})";
        }
    }
}
