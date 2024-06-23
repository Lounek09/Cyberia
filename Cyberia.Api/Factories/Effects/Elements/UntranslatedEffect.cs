using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record UntranslatedEffect : Effect
{
    public EffectParameters Parameters { get; init; }

    internal UntranslatedEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, EffectParameters parameters)
        : base(id, duration, probability, criteria, effectArea)
    {
        Parameters = parameters;
    }

    public override Description GetDescription()
    {
        return GetDescription(Parameters.Param1, Parameters.Param2, Parameters.Param3, Parameters.Param4);
    }
}
