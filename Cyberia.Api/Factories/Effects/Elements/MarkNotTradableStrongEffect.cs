using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Utils;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record MarkNotTradableStrongEffect : ParameterlessEffect
{
    public DateTime Until { get; init; }

    private MarkNotTradableStrongEffect(int id, DateTime until)
        : base(id)
    {
        Until = until;
    }

    internal static MarkNotTradableStrongEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, GameDateFormatter.CreateDateTimeFromEffectParameters(parameters));
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
