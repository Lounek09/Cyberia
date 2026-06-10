using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record PetLifePointsEffect : Effect
{
    public int LifePoints { get; init; }

    private PetLifePointsEffect(int id, int lifePoints)
        : base(id)
    {
        LifePoints = lifePoints;
    }

    internal static PetLifePointsEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, LifePoints);
    }
}
