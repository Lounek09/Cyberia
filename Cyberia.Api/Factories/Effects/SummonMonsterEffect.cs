﻿using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed class SummonMonsterEffect : BasicEffect
    {
        public int MonsterId { get; init; }
        public int Grade { get; init; }

        public SummonMonsterEffect(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area) :
            base(effectId, parameters, duration, probability, criteria, area)
        {
            MonsterId = parameters.Param2;
            Grade = parameters.Param1;
        }

        public static new SummonMonsterEffect Create(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area)
        {
            return new(effectId, parameters, duration, probability, criteria, area);
        }

        public Monster? GetMonster()
        {
            return DofusApi.Instance.Datacenter.MonstersData.GetMonsterById(MonsterId);
        }

        public override string GetDescription()
        {
            string monsterName = DofusApi.Instance.Datacenter.MonstersData.GetMonsterNameById(MonsterId);

            return GetDescriptionFromParameters(monsterName, Grade.ToString());
        }
    }
}