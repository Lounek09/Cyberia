using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Utils;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record PetsLastMealEffect : Effect
{
    public DateTime LastFedAt { get; init; }

    private PetsLastMealEffect(int id, DateTime lastFedAt)
        : base(id)
    {
        LastFedAt = lastFedAt;
    }

    internal static PetsLastMealEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, GameDateFormatter.CreateDateTimeFromEffectParameters(parameters));
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, LastFedAt.ToShortRolePlayString(culture));
    }
}
