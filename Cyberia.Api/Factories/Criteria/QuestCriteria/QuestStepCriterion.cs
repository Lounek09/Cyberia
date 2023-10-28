using Cyberia.Api.Data;

namespace Cyberia.Api.Factories.Criteria.QuestCriteria
{
    public sealed record QuestStepCriterion : Criterion, ICriterion<QuestStepCriterion>
    {
        public int QuestStepId { get; init; }

        private QuestStepCriterion(string id, char @operator, int questStepId) :
            base(id, @operator)
        {
            QuestStepId = questStepId;
        }

        public static QuestStepCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 0 && int.TryParse(parameters[0], out int questStepId))
            {
                return new(id, @operator, questStepId);
            }

            return null;
        }

        public QuestStepData? GetQuestStepData()
        {
            return DofusApi.Datacenter.QuestsData.GetQuestStepDataById(QuestStepId);
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.QuestStep.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            string questStepName = DofusApi.Datacenter.QuestsData.GetQuestStepNameById(QuestStepId);

            return GetDescription(questStepName);
        }
    }
}
