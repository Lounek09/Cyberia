using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterAddAppearanceEffect : Effect
{
    public int SpriteId { get; init; }

    private CharacterAddAppearanceEffect(int id, int spriteId)
        : base(id)
    {
        SpriteId = spriteId;
    }

    internal static CharacterAddAppearanceEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, SpriteId);
    }
}
