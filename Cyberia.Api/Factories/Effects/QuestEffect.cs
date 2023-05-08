using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed class QuestEffect : BasicEffect
    {
        public int QuestId { get; init; }

        public QuestEffect(int effectId, EffectParameters parameters, int duration, int probability, Area area) : 
            base(effectId, parameters, duration, probability, area)
        {
            QuestId = parameters.Param3;
        }

        public static new QuestEffect Create(int effectId, EffectParameters parameters, int remainingTurn, int probability, Area area)
        {
            return new(effectId, parameters, remainingTurn, probability, area);
        }

        public Quest? GetQuest()
        {
            return DofusApi.Instance.Datacenter.QuestsData.GetQuestById(QuestId);
        }

        public override string GetDescription()
        {
            string questName = DofusApi.Instance.Datacenter.QuestsData.GetQuestNameById(QuestId);

            return GetDescriptionFromParameters(null, null, questName);
        }
    }
}
