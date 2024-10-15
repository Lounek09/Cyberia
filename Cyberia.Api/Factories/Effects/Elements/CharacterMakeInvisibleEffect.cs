using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterMakeInvisibleEffect : ParameterlessEffect
{
    private CharacterMakeInvisibleEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
        : base(id, duration, probability, criteria, effectArea)
    {

    }

    internal static CharacterMakeInvisibleEffect Create(int effectId, EffectParameters _, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea);
    }
}
