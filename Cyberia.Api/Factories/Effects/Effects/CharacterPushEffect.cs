using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterPushEffect : Effect, IEffect<CharacterPushEffect>
{
    public int Distance { get; init; }

    private CharacterPushEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int distance)
        : base(id, duration, probability, criteria, effectArea)
    {
        Distance = distance;
    }

    public static CharacterPushEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1);
    }

    public Description GetDescription()
    {
        return GetDescription(Distance);
    }
}
