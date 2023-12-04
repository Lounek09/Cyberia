using Cyberia.Api.Data.States;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record MinMaxEffect : Effect, IEffect<MinMaxEffect>
{
    public int Min { get; init; }
    public int Max { get; init; }

    private MinMaxEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int min, int max)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        Min = min;
        Max = max;
    }

    public static MinMaxEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param2);
    }

    public Description GetDescription()
    {
        return GetDescription(Min, Max == 0 ? null : Max);
    }
}
