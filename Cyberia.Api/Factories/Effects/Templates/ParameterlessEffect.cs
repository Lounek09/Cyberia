using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects.Templates;

public abstract record ParameterlessEffect : Effect
{
    protected ParameterlessEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
        : base(id, duration, probability, criteria, effectArea)
    {

    }

    public Description GetDescription()
    {
        return base.GetDescription();
    }
}
