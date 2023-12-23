using Cyberia.Api.Data.States;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record FightSetStateEffect : Effect, IEffect<FightSetStateEffect>
{
    public int StateId { get; init; }

    private FightSetStateEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int stateId)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        StateId = stateId;
    }

    public static FightSetStateEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public StateData? GetStateData()
    {
        return DofusApi.Datacenter.StatesData.GetStateDataById(StateId);
    }

    public Description GetDescription()
    {
        var stateName = DofusApi.Datacenter.StatesData.GetStateNameById(StateId);

        return GetDescription(null, null, stateName);
    }
}
