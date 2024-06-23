using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record ItemBuffChangeDurationEffect : Effect
{
    public int TurnDuration { get; init; }

    private ItemBuffChangeDurationEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int turnDuration)
        : base(id, duration, probability, criteria, effectArea)
    {
        TurnDuration = turnDuration;
    }

    internal static ItemBuffChangeDurationEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public override Description GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, TurnDuration);
    }
}
