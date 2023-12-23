using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterLifePointsLostNoBoostEffect : MinMaxEffect, IEffect<CharacterLifePointsLostNoBoostEffect>
{
    private CharacterLifePointsLostNoBoostEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int min, int max)
        : base(effectId, duration, probability, criteria, effectArea, min, max)
    {
    
    }

    public static CharacterLifePointsLostNoBoostEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param2);
    }
}
