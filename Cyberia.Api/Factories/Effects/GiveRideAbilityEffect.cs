using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed class GiveRideAbilityEffect : BasicEffect
    {
        public int RideAbilityId { get; init; }

        public GiveRideAbilityEffect(int effectId, EffectParameters parameters, int duration, int probability, Area area) : 
            base(effectId, parameters, duration, probability, area)
        {
            RideAbilityId = parameters.Param3;
        }

        public static new GiveRideAbilityEffect Create(int effectId, EffectParameters parameters, int duration, int probability, Area area)
        {
            return new(effectId, parameters, duration, probability, area);
        }

        public RideAbility? GetRideAbility()
        {
            return DofusApi.Instance.Datacenter.RidesData.GetRideAbilityById(RideAbilityId);
        }

        public override string GetDescription()
        {
            string rideAbilityName = DofusApi.Instance.Datacenter.RidesData.GetRideAbilityNameById(RideAbilityId);

            return GetDescriptionFromParameters(null, null, rideAbilityName);
        }
    }
}
