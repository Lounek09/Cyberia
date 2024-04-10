using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record ChangeColorsEffect : ParameterlessEffect, IEffect
{
    private ChangeColorsEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
        : base(id, duration, probability, criteria, effectArea)
    {

    }

    internal static ChangeColorsEffect Create(int effectId, EffectParameters _, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea);
    }
}
