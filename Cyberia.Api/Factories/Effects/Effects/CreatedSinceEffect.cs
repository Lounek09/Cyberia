using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CreatedSinceEffect : Effect, IEffect<CreatedSinceEffect>
{
    public int Days { get; init; }

    private CreatedSinceEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int days)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        Days = days;
    }

    public static CreatedSinceEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(null, null, Days);
    }
}
