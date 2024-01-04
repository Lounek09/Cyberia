using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record SizeEffect
    : Effect, IEffect
{
    public int MinSize { get; init; }
    public int MaxSize { get; init; }
    public int ActualSize { get; init; }

    private SizeEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int min, int max, int actualSize)
        : base(id, duration, probability, criteria, effectArea)
    {
        MinSize = min;
        MaxSize = max;
        ActualSize = actualSize;
    }

    internal static SizeEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param2, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(MinSize, MaxSize, ActualSize);
    }
}
