using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterLifePointsLostFromEarthEffect : MinMaxEffect, IEffect
{
    private CharacterLifePointsLostFromEarthEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int min, int max)
        : base(id, duration, probability, criteria, effectArea, min, max)
    {

    }

    internal static CharacterLifePointsLostFromEarthEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1, (int)parameters.Param2);
    }
}
