using Cyberia.Api.Data;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public sealed record UntranslatedQuestObjective : QuestObjective, IQuestObjective<UntranslatedQuestObjective>
    {
        private UntranslatedQuestObjective(QuestObjectiveData questObjectiveData) :
            base(questObjectiveData)
        {

        }

        public static UntranslatedQuestObjective Create(QuestObjectiveData questObjectiveData)
        {
            return new(questObjectiveData);
        }

        public Description GetDescription()
        {
            return GetDescription(QuestObjectiveData.Parameters.ToArray());
        }
    }
}
