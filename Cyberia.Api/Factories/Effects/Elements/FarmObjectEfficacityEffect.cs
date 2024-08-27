using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record FarmObjectEfficacityEffect : Effect
{
    public int Effectiveness { get; init; }

    private FarmObjectEfficacityEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int effectiveness)
        : base(id, duration, probability, criteria, effectArea)
    {
        Effectiveness = effectiveness;
    }

    internal static FarmObjectEfficacityEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, Effectiveness);
    }
}
