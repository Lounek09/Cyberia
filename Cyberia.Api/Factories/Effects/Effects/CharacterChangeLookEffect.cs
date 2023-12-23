using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterChangeLookEffect : Effect, IEffect<CharacterChangeLookEffect>
{
    public int SpriteId { get; init; }

    private CharacterChangeLookEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int spriteId)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        SpriteId = spriteId;
    }

    public static CharacterChangeLookEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(null, null, SpriteId);
    }
}
