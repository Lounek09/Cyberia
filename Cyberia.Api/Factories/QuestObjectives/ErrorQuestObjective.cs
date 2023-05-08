using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public sealed class ErrorQuestObjective : BasicQuestObjective
    {
        public ErrorQuestObjective(QuestObjective questObjective) :
            base(questObjective)
        {

        }

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
