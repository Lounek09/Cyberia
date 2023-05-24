using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record TeleportMonsterEffect : BasicEffect
    {
        public int MaximumDistance { get; init; }
        public int MonsterId { get; init; }

        public TeleportMonsterEffect(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area) :
            base(effectId, parameters, duration, probability, criteria, area)
        {
            MaximumDistance = parameters.Param1;
            MonsterId = parameters.Param3;
        }

        public static new TeleportMonsterEffect Create(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area)
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

            return GetDescriptionFromParameters(MaximumDistance.ToString(), null, monsterName);
        }
    }
}
