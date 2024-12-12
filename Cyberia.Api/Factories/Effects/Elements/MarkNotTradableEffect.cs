using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Managers;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record MarkNotTradableEffect : Effect
{
    public DateTime DateTime { get; init; }

    private MarkNotTradableEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, DateTime dateTime)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        DateTime = dateTime;
    }

    internal static MarkNotTradableEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, dispellable, effectArea, DateTimeManager.CreateDateTimeFromEffectParameters(parameters));
    }

    public bool IsLinkedToAccount()
    {
        return DateTime == DateTime.MaxValue;
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        if (IsLinkedToAccount())
        {
            return GetDescription(culture, Translation.Get<ApiTranslations>("Effect.LinkedToAccount", culture));
        }

        return GetDescription(culture, DateTime.ToString(culture?.DateTimeFormat ?? CultureInfo.CurrentCulture.DateTimeFormat));
    }
}
