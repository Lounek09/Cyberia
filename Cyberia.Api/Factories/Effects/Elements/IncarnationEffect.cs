using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record IncarnationEffect
    : Effect, IEffect
{
    public int Level { get; init; }

    private IncarnationEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int level)
        : base(id, duration, probability, criteria, effectArea)
    {
        Level = level;
    }

    internal static IncarnationEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, Level);
    }
}
