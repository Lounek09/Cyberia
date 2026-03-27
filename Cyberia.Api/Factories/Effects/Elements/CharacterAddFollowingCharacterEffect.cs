using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterAddFollowingCharacterEffect : Effect
{
    public int SpriteId { get; init; }

    private CharacterAddFollowingCharacterEffect(int id, int spriteId)
        : base(id)
    {
        SpriteId = spriteId;
    }

    internal static CharacterAddFollowingCharacterEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, SpriteId);
    }
}
