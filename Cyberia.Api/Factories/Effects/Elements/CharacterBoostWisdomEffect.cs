using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterBoostWisdomEffect : MinMaxEffect, IRuneGeneratorEffect
{
    public int RuneId { get; init; }

    private CharacterBoostWisdomEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, int min, int max)
        : base(id, duration, probability, criteria, dispellable, effectArea, min, max)
    {
        RuneId = 2;
    }

    internal static CharacterBoostWisdomEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, dispellable, effectArea, (int)parameters.Param1, (int)parameters.Param2);
    }
}
