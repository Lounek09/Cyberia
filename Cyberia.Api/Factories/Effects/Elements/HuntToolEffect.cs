using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Templates;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record HuntToolEffect : ParameterlessEffect, IRuneGeneratorEffect
{
    public int RuneId { get; init; }

    private HuntToolEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        RuneId = 31;
    }

    public int GetRandomValue()
    {
        return 1;
    }

    internal static HuntToolEffect Create(int effectId, EffectParameters _, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, dispellable, effectArea);
    }
}
