using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record RideCertificateValidityEffect : Effect
{
    public int Days { get; init; }
    public int Hours { get; init; }
    public int Minutes { get; init; }

    private RideCertificateValidityEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int days, int hours, int minutes)
        : base(id, duration, probability, criteria, effectArea)
    {
        Days = days;
        Hours = hours;
        Minutes = minutes;
    }

    internal static RideCertificateValidityEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1, (int)parameters.Param2, (int)parameters.Param3);
    }

    public override Description GetDescription()
    {
        return GetDescription(Days, Hours, Minutes);
    }
}
