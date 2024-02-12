using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record ItemFragmentCompletionEffect
    : Effect, IEffect
{
    public int ValueInt { get; init; }
    public int ValueDecimal { get; init; }

    private ItemFragmentCompletionEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int valueInt, int valueDecimal)
        : base(id, duration, probability, criteria, effectArea)
    {
        ValueInt = valueInt;
        ValueDecimal = valueDecimal;
    }

    internal static ItemFragmentCompletionEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param2, (int)parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(string.Empty, ValueInt, ValueDecimal);
    }
}
