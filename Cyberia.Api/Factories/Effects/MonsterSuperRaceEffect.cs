using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed class MonsterSuperRaceEffect : BasicEffect
    {
        public int MonsterSuperRaceId { get; init; }

        public MonsterSuperRaceEffect(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area) : 
            base(effectId, parameters, duration, probability, criteria, area)
        {
            MonsterSuperRaceId = parameters.Param1;
        }

        public static new MonsterSuperRaceEffect Create(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area)
        {
            return new(effectId, parameters, duration, probability, criteria, area);
        }

        public MonsterSuperRace? GetMonsterSuperRace()
        {
            return DofusApi.Instance.Datacenter.MonstersData.GetMonsterSuperRaceById(MonsterSuperRaceId);
        }

        public override string GetDescription()
        {
            string monsterSuperRace = DofusApi.Instance.Datacenter.MonstersData.GetMonsterSuperRaceNameById(MonsterSuperRaceId);

            return GetDescriptionFromParameters(monsterSuperRace);
        }
    }
}
