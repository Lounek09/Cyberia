using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects.Templates;

public abstract record ParameterlessEffect : Effect
{
    protected ParameterlessEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
        : base(effectId, duration, probability, criteria, effectArea)
    {

    }

    public Description GetDescription()
    {
        return base.GetDescription();
    }
}
