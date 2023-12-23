using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record GladiatroolBoostExtraEffectEffect : ParameterlessEffect, IEffect<GladiatroolBoostExtraEffectEffect>
{
    private GladiatroolBoostExtraEffectEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
        : base(effectId, duration, probability, criteria, effectArea)
    {

    }

    public static GladiatroolBoostExtraEffectEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea);
    }
}
