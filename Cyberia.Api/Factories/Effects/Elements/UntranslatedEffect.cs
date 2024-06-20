using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record UntranslatedEffect : Effect, IEffect
{
    public EffectParameters Parameters { get; init; }

    private UntranslatedEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, EffectParameters parameters)
        : base(id, duration, probability, criteria, effectArea)
    {
        Parameters = parameters;
    }

    internal static UntranslatedEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters);
    }

    public Description GetDescription()
    {
        return GetDescription(Parameters.Param1, Parameters.Param2, Parameters.Param3, Parameters.Param4);
    }
}
