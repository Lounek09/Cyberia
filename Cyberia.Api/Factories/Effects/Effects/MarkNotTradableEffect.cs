using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record MarkNotTradableEffect : Effect, IEffect<MarkNotTradableEffect>
{
    public DateTime DateTime { get; init; }

    private MarkNotTradableEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, DateTime dateTime)
        : base(id, duration, probability, criteria, effectArea)
    {
        DateTime = dateTime;
    }

    public static MarkNotTradableEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, DateManager.GetDateTimeFromEffectParameters(parameters));
    }

    public bool IsLinkedToAccount()
    {
        return DateTime == DateTime.MaxValue;
    }

    public Description GetDescription()
    {
        if (IsLinkedToAccount())
        {
            return GetDescription(Resources.Effect_LinkedToAccount);
        }

        return GetDescription(DateTime.ToString("dd/MM/yyy HH:mm"));
    }
}
