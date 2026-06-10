using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record GladiatroolGiveTokenAtEndFightEffect : Effect
{
    public int Quantity { get; init; }

    private GladiatroolGiveTokenAtEndFightEffect(int id, int quantity)
        : base(id)
    {
        Quantity = quantity;
    }

    internal static GladiatroolGiveTokenAtEndFightEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, Quantity);
    }
}
