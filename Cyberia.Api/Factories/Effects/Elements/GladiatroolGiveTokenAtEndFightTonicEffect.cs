using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record GladiatroolGiveTokenAtEndFightTonicEffect : Effect
{
    public int CurrentTonicBuy { get; init; }
    public int MaxTonicBuyable { get; init; }
    public int Quantity { get; init; }

    private GladiatroolGiveTokenAtEndFightTonicEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, int currentTonicBuy, int maxTonicBuyable, int quantity)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        CurrentTonicBuy = currentTonicBuy;
        MaxTonicBuyable = maxTonicBuyable;
        Quantity = quantity;
    }

    internal static GladiatroolGiveTokenAtEndFightTonicEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, dispellable, effectArea, (int)parameters.Param1, (int)parameters.Param2, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, CurrentTonicBuy, MaxTonicBuyable, Quantity);
    }
}
