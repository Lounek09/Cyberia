using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects.Templates;

public abstract record MinMaxEffect : Effect
{
    public int Min { get; init; }
    public int Max { get; init; }

    protected MinMaxEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int min, int max)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        Min = min;
        Max = max;
    }

    public Description GetDescription()
    {
        return GetDescription(Min, Max == 0 ? null : Max);
    }
}
