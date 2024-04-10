using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterChangeLookEffect
    : Effect, IEffect
{
    public int SpriteId { get; init; }

    private CharacterChangeLookEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int spriteId)
        : base(id, duration, probability, criteria, effectArea)
    {
        SpriteId = spriteId;
    }

    internal static CharacterChangeLookEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, SpriteId);
    }
}
