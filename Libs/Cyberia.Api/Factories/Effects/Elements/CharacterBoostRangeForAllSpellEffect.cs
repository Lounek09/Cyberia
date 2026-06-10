using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterBoostRangeForAllSpellEffect : Effect
{
    public int Range { get; init; }

    private CharacterBoostRangeForAllSpellEffect(int id, int range)
        : base(id)
    {
        Range = range;
    }

    internal static CharacterBoostRangeForAllSpellEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)

    {
        return GetDescription(culture, string.Empty, string.Empty, Range);
    }
}
