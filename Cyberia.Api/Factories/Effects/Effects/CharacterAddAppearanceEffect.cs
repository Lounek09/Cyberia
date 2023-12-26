using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterAddAppearanceEffect : Effect, IEffect<CharacterAddAppearanceEffect>
{
    public int SpriteId { get; init; }

    private CharacterAddAppearanceEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int spriteId)
        : base(id, duration, probability, criteria, effectArea)
    {
        SpriteId = spriteId;
    }

    public static CharacterAddAppearanceEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(SpriteId);
    }
}
