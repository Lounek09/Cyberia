using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterAddAppearanceEffect : Effect, IEffect
{
    public int SpriteId { get; init; }

    private CharacterAddAppearanceEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int spriteId)
        : base(id, duration, probability, criteria, effectArea)
    {
        SpriteId = spriteId;
    }

    internal static CharacterAddAppearanceEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(SpriteId);
    }
}
