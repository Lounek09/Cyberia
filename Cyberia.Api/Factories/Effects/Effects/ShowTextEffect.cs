using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record ShowTextEffect : Effect, IEffect<ShowTextEffect>
{
    public string Value { get; init; }

    private ShowTextEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, string value)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        Value = value;
    }

    public static ShowTextEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param4);
    }

    public Description GetDescription()
    {
        return GetDescription(null, null, null, Value);
    }
}
