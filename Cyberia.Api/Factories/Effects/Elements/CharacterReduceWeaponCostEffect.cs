using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterReduceWeaponCostEffect : Effect
{
    public int ActionPointsReduced { get; init; }

    private CharacterReduceWeaponCostEffect(int id, int actionPointsReduced)
        : base(id)
    {
        ActionPointsReduced = actionPointsReduced;
    }

    internal static CharacterReduceWeaponCostEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, ActionPointsReduced);
    }
}
