using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Utils;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record ItemChangeDurationEffect : Effect
{
    public DateTime DateTime { get; init; }

    private ItemChangeDurationEffect(int id, DateTime dateTime)
        : base(id)
    {
        DateTime = dateTime;
    }

    internal static ItemChangeDurationEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, GameDateFormatter.CreateDateTimeFromEffectParameters(parameters));
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, DateTime.ToShortRolePlayString(culture));
    }
}
