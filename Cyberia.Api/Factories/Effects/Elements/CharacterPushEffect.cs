using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterPushEffect : Effect, IEffect
{
    public int Distance { get; init; }

    private CharacterPushEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int distance)
        : base(id, duration, probability, criteria, effectArea)
    {
        Distance = distance;
    }

    internal static CharacterPushEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1);
    }

    public Description GetDescription()
    {
        return GetDescription(Distance);
    }
}
