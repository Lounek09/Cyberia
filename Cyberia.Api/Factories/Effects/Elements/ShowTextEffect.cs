using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record ShowTextEffect : Effect, IEffect
{
    public string Value { get; init; }

    private ShowTextEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, string value)
        : base(id, duration, probability, criteria, effectArea)
    {
        Value = value;
    }

    internal static ShowTextEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param4);
    }

    public Description GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, string.Empty, Value);
    }
}
