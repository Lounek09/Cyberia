using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record SizeEffect : Effect, IEffect<SizeEffect>
{
    public int Min { get; init; }
    public int Max { get; init; }
    public int Value { get; init; }

    private SizeEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int min, int max, int value)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        Min = min;
        Max = max;
        Value = value;
    }

    public static SizeEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param2, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(Min, Max, Value);
    }
}
