using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CreatedSinceEffect : Effect
{
    public int Days { get; init; }

    private CreatedSinceEffect(int id, int days)
        : base(id)
    {
        Days = days;
    }

    internal static CreatedSinceEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, Days);
    }
}
