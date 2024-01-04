using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects.Templates;

public abstract record MinMaxEffect
    : Effect
{
    public int Min { get; init; }
    public int Max { get; init; }

    protected MinMaxEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int min, int max)
        : base(id, duration, probability, criteria, effectArea)
    {
        Min = min;
        Max = max;
    }

    public int GetRandomValue()
    {
        return Max < Min ? Min : Random.Shared.Next(Min, Max + 1);
    }

    public Description GetDescription()
    {
        return GetDescription(Min, Max == 0 ? string.Empty : Max.ToString());
    }
}
