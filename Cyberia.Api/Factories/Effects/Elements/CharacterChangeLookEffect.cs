using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterChangeLookEffect : Effect
{
    public int SpriteId { get; init; }

    private CharacterChangeLookEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, int spriteId)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        SpriteId = spriteId;
    }

    internal static CharacterChangeLookEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, dispellable, effectArea, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var spriteName = SpriteId == -1
            ? Translation.Get<ApiTranslations>("OriginalAppearance", culture)
            : SpriteId.ToString();

        return GetDescription(culture, string.Empty, string.Empty, spriteName);
    }
}
