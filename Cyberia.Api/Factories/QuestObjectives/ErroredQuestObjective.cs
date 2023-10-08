using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public sealed record ErroredQuestObjective : QuestObjective, IQuestObjective<ErroredQuestObjective>
    {
        private ErroredQuestObjective(QuestObjectiveData questObjectiveData) :
            base(questObjectiveData)
        {

        }

        public static ErroredQuestObjective Create(QuestObjectiveData questObjectiveData)
        {
            return new(questObjectiveData);
        }

        public Description GetDescription()
        {
            return GetDescription(QuestObjectiveData.Parameters.ToArray());
        }
    }
}
