using Cyberia.Api.Data;

namespace Cyberia.Api.Factories.Criteria.QuestCriteria
{
    public sealed record QuestCriterion : Criterion, ICriterion<QuestCriterion>
    {
        public int QuestId { get; init; }

        private QuestCriterion(string id, char @operator, int questId) :
            base(id, @operator)
        {
            QuestId = questId;
        }

        public static QuestCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 0 && int.TryParse(parameters[0], out int questId))
            {
                return new(id, @operator, questId);
            }

            return null;
        }

        public QuestData? GetQuestData()
        {
            return DofusApi.Instance.Datacenter.QuestsData.GetQuestDataById(QuestId);
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.Quest.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            string questName = DofusApi.Instance.Datacenter.QuestsData.GetQuestNameById(QuestId);

            return GetDescription(questName);
        }
    }
}
