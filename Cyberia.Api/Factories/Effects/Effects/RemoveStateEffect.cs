using Cyberia.Api.Data;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record RemoveStateEffect : Effect, IEffect<RemoveStateEffect>
    {
        public int StateId { get; init; }

        private RemoveStateEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int stateId) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            StateId = stateId;
        }

        public static RemoveStateEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
        }

        public StateData? GetStateData()
        {
            return DofusApi.Instance.Datacenter.StatesData.GetStateDataById(StateId);
        }

        public Description GetDescription()
        {
            string stateName = DofusApi.Instance.Datacenter.StatesData.GetStateNameById(StateId);

            return GetDescription(null, null, stateName);
        }
    }
}
