using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record ItemBuffChangeDurationEffect : Effect, IEffect<ItemBuffChangeDurationEffect>
{
    public int TurnDuration { get; init; }

    private ItemBuffChangeDurationEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int turnDuration)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        TurnDuration = turnDuration;
    }

    public static ItemBuffChangeDurationEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(null, null, Duration);
    }
}
