using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Utils;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record LockToAccountUntilEffect : Effect
{
    public DateTime DateTime { get; init; }

    private LockToAccountUntilEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, DateTime dateTime)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        DateTime = dateTime;
    }

    internal static LockToAccountUntilEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, dispellable, effectArea, GameDateFormatter.CreateDateTimeFromEffectParameters(parameters));
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, DateTime.ToShortRolePlayString(culture));
    }
}
