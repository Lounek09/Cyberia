using Cyberia.Api.Data;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record TransformIntoMonsterEffect : Effect, IEffect<TransformIntoMonsterEffect>
    {
        public int MonsterId { get; init; }

        private TransformIntoMonsterEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int monsterId) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            MonsterId = monsterId;
        }

        public static TransformIntoMonsterEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, parameters.Param1);
        }

        public MonsterData? GetMonsterData()
        {
            return DofusApi.Datacenter.MonstersData.GetMonsterDataById(MonsterId);
        }

        public Description GetDescription()
        {
            string monsterName = DofusApi.Datacenter.MonstersData.GetMonsterNameById(MonsterId);

            return GetDescription(monsterName);
        }
    }
}
