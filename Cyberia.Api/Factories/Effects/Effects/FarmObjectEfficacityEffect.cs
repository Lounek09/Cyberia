using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record FarmObjectEfficacityEffect : Effect, IEffect<FarmObjectEfficacityEffect>
{
    public int Effectiveness { get; init; }

    private FarmObjectEfficacityEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int effectiveness)
        : base(id, duration, probability, criteria, effectArea)
    {
        Effectiveness = effectiveness;
    }

    public static FarmObjectEfficacityEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(null, null, Effectiveness);
    }
}
