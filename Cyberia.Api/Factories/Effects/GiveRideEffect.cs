using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed class GiveRideEffect : BasicEffect
    {
        public int RideId { get; init; }
        public int RideAbilityId { get; init; }
        public bool Infertile { get; init; }

        public GiveRideEffect(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area) :
            base(effectId, parameters, duration, probability, criteria, area)
        {
            RideId = parameters.Param3;
            RideAbilityId = parameters.Param2;
            Infertile = parameters.Param1 == 1;
        }

        public static new BasicEffect Create(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area)
        {
            return new(effectId, parameters, duration, probability, criteria, area);
        }

        public Ride? GetRide()
        {
            return DofusApi.Instance.Datacenter.RidesData.GetRideById(RideId);
        }

        public RideAbility? GetRideAbility()
        {
            return DofusApi.Instance.Datacenter.RidesData.GetRideAbilityById(RideAbilityId);
        }

        public override string GetDescription()
        {
            string value = DofusApi.Instance.Datacenter.RidesData.GetRideNameById(RideId);
            RideAbility? rideAbility = GetRideAbility();
            if (rideAbility is not null)
                value += $" {rideAbility.Name}";
            if (Infertile)
                value += " Stérile";

            return GetDescriptionFromParameters(value);
        }
    }
}
