using Cyberia.Api.Data;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record MonsterKillCounterEffect : Effect, IEffect<MonsterKillCounterEffect>
    {
        public int MonsterId { get; init; }
        public int Count { get; init; }

        private MonsterKillCounterEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int monsterId, int count) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            MonsterId = monsterId;
            Count = count;
        }

        public static MonsterKillCounterEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param3);
        }

        public MonsterData? GetMonsterData()
        {
            return DofusApi.Instance.Datacenter.MonstersData.GetMonsterDataById(MonsterId);
        }

        public Description GetDescription()
        {
            string monsterName = DofusApi.Instance.Datacenter.MonstersData.GetMonsterNameById(MonsterId);

            return GetDescription(monsterName, null, Count);
        }
    }
}
