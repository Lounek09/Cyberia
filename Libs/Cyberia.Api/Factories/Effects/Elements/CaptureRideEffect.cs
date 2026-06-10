using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CaptureRideEffect : Effect
{
    public int CapturePercent { get; init; }

    private CaptureRideEffect(int id, int capturePercent)
        : base(id)
    {
        CapturePercent = capturePercent;
    }

    internal static CaptureRideEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param1);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, CapturePercent);
    }
}
