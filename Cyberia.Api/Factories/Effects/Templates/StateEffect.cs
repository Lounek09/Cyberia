using Cyberia.Api.Data.States;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects.Templates;

public abstract record StateEffect : Effect
{
    public int StateId { get; init; }

    protected StateEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int stateId)
        : base(id, duration, probability, criteria, effectArea)
    {
        StateId = stateId;
    }

    public StateData? GetStateData()
    {
        return DofusApi.Datacenter.StatesRepository.GetStateDataById(StateId);
    }

    public Description GetDescription()
    {
        var stateName = DofusApi.Datacenter.StatesRepository.GetStateNameById(StateId);

        return GetDescription(string.Empty, string.Empty, stateName);
    }
}
