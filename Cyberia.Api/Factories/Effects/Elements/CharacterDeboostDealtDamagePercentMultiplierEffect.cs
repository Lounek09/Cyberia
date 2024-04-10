using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterDeboostDealtDamagePercentMultiplierEffect : MinMaxEffect, IEffect
{
    private CharacterDeboostDealtDamagePercentMultiplierEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int min, int max)
        : base(id, duration, probability, criteria, effectArea, min, max)
    {

    }

    internal static CharacterDeboostDealtDamagePercentMultiplierEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1, (int)parameters.Param2);
    }
}
