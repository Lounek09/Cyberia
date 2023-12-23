using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects.Templates;

public abstract record ParameterlessEffect : Effect
{
    protected ParameterlessEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
        : base(id, duration, probability, criteria, effectArea)
    {

    }

    public Description GetDescription()
    {
        return base.GetDescription();
    }
}
