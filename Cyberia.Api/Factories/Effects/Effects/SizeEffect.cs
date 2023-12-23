using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record SizeEffect : Effect, IEffect<SizeEffect>
{
    public int MinSize { get; init; }
    public int MaxSize { get; init; }
    public int ActualSize { get; init; }

    private SizeEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int min, int max, int actualSize)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        MinSize = min;
        MaxSize = max;
        ActualSize = actualSize;
    }

    public static SizeEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param2, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(MinSize, MaxSize, ActualSize);
    }
}
