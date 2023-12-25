using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterBoostActionPointsEffect : MinMaxEffect, IEffect<CharacterBoostActionPointsEffect>, IRuneGeneratorEffect
{
    public int RuneId { get; init; }

    private CharacterBoostActionPointsEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int min, int max)
        : base(id, duration, probability, criteria, effectArea, min, max)
    {
        RuneId = 7;
    }

    public static CharacterBoostActionPointsEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param2);
    }
}
