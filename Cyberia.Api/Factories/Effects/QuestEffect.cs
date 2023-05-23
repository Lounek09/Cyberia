using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record QuestEffect : BasicEffect
    {
        public int QuestId { get; init; }

        public QuestEffect(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area) :
            base(effectId, parameters, duration, probability, criteria, area)
        {
            QuestId = parameters.Param3;
        }

        public static new QuestEffect Create(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area)
        {
            return new(effectId, parameters, duration, probability, criteria, area);
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
