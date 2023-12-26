using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record ItemFragmentCompletionEffect : Effect, IEffect<ItemFragmentCompletionEffect>
{
    public int ValueInt { get; init; }
    public int ValueDecimal { get; init; }

    private ItemFragmentCompletionEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int valueInt, int valueDecimal)
        : base(id, duration, probability, criteria, effectArea)
    {
        ValueInt = valueInt;
        ValueDecimal = valueDecimal;
    }

    public static ItemFragmentCompletionEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param2, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(null, ValueInt, ValueDecimal);
    }
}
