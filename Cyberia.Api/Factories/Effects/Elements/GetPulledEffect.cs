using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record GetPulledEffect : Effect
{
    public int Distance { get; init; }

    private GetPulledEffect(int id, int distance)
        : base(id)
    {
        Distance = distance;
    }

    internal static GetPulledEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param1);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Distance);
    }
}
