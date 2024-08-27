using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record LockToAccountUntilEffect : Effect
{
    public DateTime DateTime { get; init; }

    private LockToAccountUntilEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, DateTime dateTime)
        : base(id, duration, probability, criteria, effectArea)
    {
        DateTime = dateTime;
    }

    internal static LockToAccountUntilEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, DateManager.CreateDateTimeFromEffectParameters(parameters));
    }

    public override DescriptionString GetDescription()
    {
        return GetDescription(DateTime.ToString("dd/MM/yyy HH:mm"));
    }
}
