using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterChangeLookEffect : Effect
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

    public override Description GetDescription()
    {
        var spriteName = SpriteId == -1
            ? ApiTranslations.OriginalAppearance
            : SpriteId.ToString();

        return GetDescription(string.Empty, string.Empty, spriteName);
    }
}
