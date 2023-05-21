using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public sealed record ErrorQuestObjective(QuestObjective QuestObjective) :
        BasicQuestObjective(QuestObjective)
    {
        public static new ErrorQuestObjective Create(QuestObjective questObjective)
        {
            return new(questObjective);
        }

        public override string GetDescription()
        {
            return $"Erreur détectée lors de l'initialisation du type d'objectif {QuestObjective.QuestObjectiveTypeId.ToString().Bold()}";
        }
    }
}
