using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record TeleportMonsterEffect : Effect, IEffect<TeleportMonsterEffect>
    {
        public int MonsterId { get; init; }
        public int MaximumDistance { get; init; }

        private TeleportMonsterEffect(int effectId, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea, int monsterId, int maximumDistance) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            MonsterId = monsterId;
            MaximumDistance = maximumDistance;
        }

        public static TeleportMonsterEffect Create(int effectId, EffectParameters parameters, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, parameters.Param3, parameters.Param1);
        }

        public MonsterData? GetMonsterData()
        {
            return DofusApi.Instance.Datacenter.MonstersData.GetMonsterDataById(MonsterId);
        }

        public Description GetDescription()
        {
            string monsterName = DofusApi.Instance.Datacenter.MonstersData.GetMonsterNameById(MonsterId);

            return GetDescription(MaximumDistance, null, monsterName);
        }
    }
}
