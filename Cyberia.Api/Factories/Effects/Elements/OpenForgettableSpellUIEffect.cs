using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Factories.Effects;

public sealed record OpenForgettableSpellUIEffect : ParameterlessEffect
{
    private OpenForgettableSpellUIEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
        : base(id, duration, probability, criteria, effectArea)
    {

    }

    internal static OpenForgettableSpellUIEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea);
    }
}
