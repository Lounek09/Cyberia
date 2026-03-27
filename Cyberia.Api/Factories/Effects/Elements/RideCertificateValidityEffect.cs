using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record RideCertificateValidityEffect : Effect
{
    public int Days { get; init; }
    public int Hours { get; init; }
    public int Minutes { get; init; }

    private RideCertificateValidityEffect(int id, int days, int hours, int minutes)
        : base(id)
    {
        Days = days;
        Hours = hours;
        Minutes = minutes;
    }

    internal static RideCertificateValidityEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param1, (int)parameters.Param2, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Days, Hours, Minutes);
    }
}
