using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record RideCertificateValidityEffect
    : Effect, IEffect
{
    public int Days { get; init; }
    public int Hours { get; init; }
    public int Minutes { get; init; }

    private RideCertificateValidityEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int days, int hours, int minutes)
        : base(id, duration, probability, criteria, effectArea)
    {
        Days = days;
        Hours = hours;
        Minutes = minutes;
    }

    internal static RideCertificateValidityEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param2, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(Days, Hours, Minutes);
    }
}
