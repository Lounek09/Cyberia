using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record QuestEffect : Effect, IEffect<QuestEffect>
    {
        public int QuestId { get; init; }

        private QuestEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int questId) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            QuestId = questId;
        }

        public static QuestEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
        }

        public QuestData? GetQuestData()
        {
            return DofusApi.Instance.Datacenter.QuestsData.GetQuestDataById(QuestId);
        }

        public Description GetDescription()
        {
            string questName = DofusApi.Instance.Datacenter.QuestsData.GetQuestNameById(QuestId);

            return GetDescription(null, null, questName);
        }
    }
}
