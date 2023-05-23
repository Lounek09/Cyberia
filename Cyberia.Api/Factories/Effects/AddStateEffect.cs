using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record AddStateEffect : BasicEffect
    {
        public int StateId { get; init; }

        public AddStateEffect(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area) :
            base(effectId, parameters, duration, probability, criteria, area)
        {
            StateId = parameters.Param3;
        }

        public static new AddStateEffect Create(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area)
        {
            return new(effectId, parameters, duration, probability, criteria, area);
        }

        public State? GetState()
        {
            return DofusApi.Instance.Datacenter.StatesData.GetStateById(StateId);
        }

        public override string GetDescription()
        {
            string stateName = DofusApi.Instance.Datacenter.StatesData.GetStateNameById(StateId);

            return GetDescriptionFromParameters(null, null, stateName);
        }
    }
}
