using Cyberia.Api.Data;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record MonsterSuperRaceEffect : Effect, IEffect<MonsterSuperRaceEffect>
    {
        public int MonsterSuperRaceId { get; init; }

        private MonsterSuperRaceEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int monsterSuperRaceId) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            MonsterSuperRaceId = monsterSuperRaceId;
        }

        public static MonsterSuperRaceEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, parameters.Param1);
        }

        public MonsterSuperRaceData? GetMonsterSuperRaceData()
        {
            return DofusApi.Instance.Datacenter.MonstersData.GetMonsterSuperRaceDataById(MonsterSuperRaceId);
        }

        public Description GetDescription()
        {
            string monsterSuperRace = DofusApi.Instance.Datacenter.MonstersData.GetMonsterSuperRaceNameById(MonsterSuperRaceId);

            return GetDescription(monsterSuperRace);
        }
    }
}
