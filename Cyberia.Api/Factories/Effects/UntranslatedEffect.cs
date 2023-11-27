using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record UntranslatedEffect : Effect, IEffect<UntranslatedEffect>
{
    EffectParameters Parameters { get; init; }

    private UntranslatedEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, EffectParameters parameters)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        Parameters = parameters;
    }

    public static UntranslatedEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters);
    }

    public Description GetDescription()
    {
        return GetDescription(Parameters.Param1, Parameters.Param2, Parameters.Param3, Parameters.Param4);
    }
}
