﻿using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record MonsterRaceEffect : Effect, IEffect<MonsterRaceEffect>
    {
        public int MonsterRaceId { get; init; }

        private MonsterRaceEffect(int effectId, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea, int monsterRaceId) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            MonsterRaceId = monsterRaceId;
        }

        public static MonsterRaceEffect Create(int effectId, EffectParameters parameters, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, parameters.Param1);
        }

        public MonsterRaceData? GetMonsterRaceData()
        {
            return DofusApi.Instance.Datacenter.MonstersData.GetMonsterRaceDataById(MonsterRaceId);
        }

        public Description GetDescription()
        {
            string monsterRaceName = DofusApi.Instance.Datacenter.MonstersData.GetMonsterRaceNameById(MonsterRaceId);

            return GetDescription(monsterRaceName);
        }
    }
}