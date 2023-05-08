using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed class MonsterEffect : BasicEffect
    {
        public int MonsterId { get; init; }

        public MonsterEffect(int effectId, EffectParameters parameters, int duration, int probability, Area area) : 
            base(effectId, parameters, duration, probability, area)
        {
            MonsterId = parameters.Param1;
        }

        public static new MonsterEffect Create(int effectId, EffectParameters parameters, int duration, int probability, Area area)
        {
            return new(effectId, parameters, duration, probability, area);
        }

        public Monster? GetMonster()
        {
            return DofusApi.Instance.Datacenter.MonstersData.GetMonsterById(MonsterId);
        }

        public override string GetDescription()
        {
            string monsterName = DofusApi.Instance.Datacenter.MonstersData.GetMonsterNameById(MonsterId);

            return GetDescriptionFromParameters(monsterName);
        }
    }
}
