using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Utils;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record LockToAccountUntilEffect : Effect
{
    public DateTime Until { get; init; }

    private LockToAccountUntilEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, DateTime until)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        Until = until;
    }

    internal static LockToAccountUntilEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, dispellable, effectArea, GameDateFormatter.CreateDateTimeFromEffectParameters(parameters));
    }

    public bool IsInfinite()
    {
        return Until == DateTime.MaxValue;
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        if (IsInfinite())
        {
            return GetDescription(culture, string.Empty);
        }

        return GetDescription(culture, Until.ToShortRolePlayString(culture));
    }
}
