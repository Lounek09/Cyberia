using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record GetPulledEffect : Effect
{
    public int Distance { get; init; }

    private GetPulledEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int distance)
        : base(id, duration, probability, criteria, effectArea)
    {
        Distance = distance;
    }

    internal static GetPulledEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1);
    }

    public override DescriptionString GetDescription()
    {
        return GetDescription(Distance);
    }
}
