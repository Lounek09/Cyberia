using Cyberia.Api.Data;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record GiveRideEffect : Effect, IEffect<GiveRideEffect>
    {
        public int RideId { get; init; }
        public int RideAbilityId { get; init; }
        public bool Infertile { get; init; }

        private GiveRideEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int rideId, int rideAbilityId, bool infertile) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            RideId = rideId;
            RideAbilityId = rideAbilityId;
            Infertile = infertile;
        }

        public static GiveRideEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, parameters.Param3, parameters.Param2, parameters.Param1 == 1);
        }

        public RideData? GetRideData()
        {
            return DofusApi.Instance.Datacenter.RidesData.GetRideDataById(RideId);
        }

        public RideAbilityData? GetRideAbilityData()
        {
            return DofusApi.Instance.Datacenter.RidesData.GetRideAbilityDataById(RideAbilityId);
        }

        public Description GetDescription()
        {
            string value = DofusApi.Instance.Datacenter.RidesData.GetRideNameById(RideId);

            RideAbilityData? rideAbility = GetRideAbilityData();
            if (rideAbility is not null)
            {
                value += $" {rideAbility.Name}";
            }

            if (Infertile)
            {
                value += $" {Resources.Infertile}";
            }

            return GetDescription(value);
        }
    }
}
