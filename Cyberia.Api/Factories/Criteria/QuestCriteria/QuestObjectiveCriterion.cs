using Cyberia.Api.Data;

namespace Cyberia.Api.Factories.Criteria.QuestCriteria
{
    public sealed record QuestObjectiveCriterion : Criterion, ICriterion<QuestObjectiveCriterion>
    {
        public int QuestObjectiveId { get; init; }

        private QuestObjectiveCriterion(string id, char @operator, int questStepId) :
            base(id, @operator)
        {
            QuestObjectiveId = questStepId;
        }

        public static QuestObjectiveCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 0 && int.TryParse(parameters[0], out int questObjectiveId))
            {
                return new(id, @operator, questObjectiveId);
            }

            return null;
        }

        public QuestObjectiveData? GetQuestObjectiveData()
        {
            return DofusApi.Datacenter.QuestsData.GetQuestObjectiveDataById(QuestObjectiveId);
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.QuestObjective.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            Description questObjectiveDescription = DofusApi.Datacenter.QuestsData.GetQuestObjectiveDescriptionById(QuestObjectiveId);

            return GetDescription(questObjectiveDescription);
        }
    }
}
