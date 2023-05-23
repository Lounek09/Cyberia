using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record MonsterRaceEffect : BasicEffect
    {
        public int MonsterRaceId { get; init; }

        public MonsterRaceEffect(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area) :
            base(effectId, parameters, duration, probability, criteria, area)
        {
            MonsterRaceId = parameters.Param1;
        }

        public static new MonsterRaceEffect Create(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area)
        {
            return new(effectId, parameters, duration, probability, criteria, area);
        }

        public MonsterRace? GetMonsterRace()
        {
            return DofusApi.Instance.Datacenter.MonstersData.GetMonsterRaceById(MonsterRaceId);
        }

        public override string GetDescription()
        {
            string monsterRaceName = DofusApi.Instance.Datacenter.MonstersData.GetMonsterRaceNameById(MonsterRaceId);

            return GetDescriptionFromParameters(monsterRaceName);
        }
    }
}
