using Cyberia.Api.Data.States;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record FightUnsetStateEffect : StateEffect, IEffect<FightUnsetStateEffect>
{
    private FightUnsetStateEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int stateId)
        : base(id, duration, probability, criteria, effectArea, stateId)
    {

    }

    public static FightUnsetStateEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }
}
