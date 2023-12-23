using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CaptureRideEffect : Effect, IEffect<CaptureRideEffect>
{
    public int CapturePercent { get; init; }

    private CaptureRideEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int capturePercent)
        : base(id, duration, probability, criteria, effectArea)
    {
        CapturePercent = capturePercent;
    }

    public static CaptureRideEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1);
    }

    public Description GetDescription()
    {
        return GetDescription(CapturePercent);
    }
}
