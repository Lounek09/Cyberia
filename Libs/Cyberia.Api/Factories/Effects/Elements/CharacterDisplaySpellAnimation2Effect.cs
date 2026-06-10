using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterDisplaySpellAnimation2Effect : Effect
{
    public int GfxId { get; init; }

    private CharacterDisplaySpellAnimation2Effect(int id, int gfxId)
        : base(id)
    {
        GfxId = gfxId;
    }

    internal static CharacterDisplaySpellAnimation2Effect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, GfxId);
    }
}
