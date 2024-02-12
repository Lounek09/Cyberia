using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CreatedSinceEffect
    : Effect, IEffect
{
    public int Days { get; init; }

    private CreatedSinceEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int days)
        : base(id, duration, probability, criteria, effectArea)
    {
        Days = days;
    }

    internal static CreatedSinceEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, Days);
    }
}
