using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CaptureSoulEffect : Effect, IEffect<CaptureSoulEffect>
{
    public int CapturePercent { get; init; }
    public int Power { get; init; }

    private CaptureSoulEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int capturePercent, int power)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        CapturePercent = capturePercent;
        Power = power;
    }

    public static CaptureSoulEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(CapturePercent, Power);
    }
}
