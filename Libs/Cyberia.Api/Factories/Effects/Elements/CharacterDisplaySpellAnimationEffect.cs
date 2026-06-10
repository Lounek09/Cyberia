using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterDisplaySpellAnimationEffect : Effect
{
    public int GfxId { get; init; }

    private CharacterDisplaySpellAnimationEffect(int id, int gfxId)
        : base(id)
    {
        GfxId = gfxId;
    }

    internal static CharacterDisplaySpellAnimationEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, GfxId);
    }
}
