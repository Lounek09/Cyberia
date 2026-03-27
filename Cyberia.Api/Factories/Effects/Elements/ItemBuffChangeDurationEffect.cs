using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record ItemBuffChangeDurationEffect : Effect
{
    public int TurnDuration { get; init; }

    private ItemBuffChangeDurationEffect(int id, int turnDuration)
        : base(id)
    {
        TurnDuration = turnDuration;
    }

    internal static ItemBuffChangeDurationEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, TurnDuration);
    }
}
