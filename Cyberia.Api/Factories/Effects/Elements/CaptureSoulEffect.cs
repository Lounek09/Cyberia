using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CaptureSoulEffect : Effect
{
    public int CapturePercent { get; init; }
    public int Power { get; init; }

    private CaptureSoulEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, int capturePercent, int power)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        CapturePercent = capturePercent;
        Power = power;
    }

    internal static CaptureSoulEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, dispellable, effectArea, (int)parameters.Param1, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, CapturePercent, string.Empty, Power);
    }
}
