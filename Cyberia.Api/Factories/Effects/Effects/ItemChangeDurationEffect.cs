using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record ItemChangeDurationEffect : Effect, IEffect<ItemChangeDurationEffect>
{
    public DateTime DateTime { get; init; }

    private ItemChangeDurationEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, DateTime dateTime)
        : base(id, duration, probability, criteria, effectArea)
    {
        DateTime = dateTime;
    }

    public static ItemChangeDurationEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, DateManager.GetDateTimeFromEffectParameters(parameters));
    }

    public Description GetDescription()
    {
        return GetDescription(DateTime.ToString("dd/MM/yyy HH:mm"));
    }
}
