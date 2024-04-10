using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record PvpLadderEffect : Effect, IEffect
{
    public int Count { get; init; }

    private PvpLadderEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int count)
        : base(id, duration, probability, criteria, effectArea)
    {
        Count = count;
    }

    internal static PvpLadderEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param2);
    }

    public Description GetDescription()
    {
        return GetDescription(string.Empty, Count);
    }
}
