using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterChangeLookEffect : Effect
{
    public int SpriteId { get; init; }

    private CharacterChangeLookEffect(int id, int spriteId)
        : base(id)
    {
        SpriteId = spriteId;
    }

    internal static CharacterChangeLookEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var spriteName = SpriteId == -1
            ? Translation.Get<ApiTranslations>("OriginalAppearance", culture)
            : SpriteId.ToString();

        return GetDescription(culture, string.Empty, string.Empty, spriteName);
    }
}
