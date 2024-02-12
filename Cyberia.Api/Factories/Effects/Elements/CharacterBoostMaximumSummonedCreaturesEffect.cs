using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterBoostMaximumSummonedCreaturesEffect
    : MinMaxEffect, IEffect, IRuneGeneratorEffect
{
    public int RuneId { get; init; }

    private CharacterBoostMaximumSummonedCreaturesEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int min, int max)
        : base(id, duration, probability, criteria, effectArea, min, max)
    {
        RuneId = 15;
    }

    internal static CharacterBoostMaximumSummonedCreaturesEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1, (int)parameters.Param2);
    }
}
