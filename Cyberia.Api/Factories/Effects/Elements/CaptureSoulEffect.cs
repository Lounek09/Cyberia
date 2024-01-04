using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CaptureSoulEffect
    : Effect, IEffect
{
    public int CapturePercent { get; init; }
    public int Power { get; init; }

    private CaptureSoulEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int capturePercent, int power)
        : base(id, duration, probability, criteria, effectArea)
    {
        CapturePercent = capturePercent;
        Power = power;
    }

    internal static CaptureSoulEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(CapturePercent, Power);
    }
}
