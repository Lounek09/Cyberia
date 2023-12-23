using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record ItemMaxSoulEaterEffect : ParameterlessEffect, IEffect<ItemMaxSoulEaterEffect>
{
    private ItemMaxSoulEaterEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
        : base(effectId, duration, probability, criteria, effectArea)
    {

    }

    public static ItemMaxSoulEaterEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea);
    }
}
