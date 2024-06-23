using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record CaptureSoulEffect : Effect
{
    public int CapturePercent { get; init; }
    public int Power { get; init; }

    private CaptureSoulEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int capturePercent, int power)
        : base(id, duration, probability, criteria, effectArea)
    {
        CapturePercent = capturePercent;
        Power = power;
    }

    internal static CaptureSoulEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1, (int)parameters.Param3);
    }

    public override Description GetDescription()
    {
        return GetDescription(CapturePercent, Power);
    }
}
