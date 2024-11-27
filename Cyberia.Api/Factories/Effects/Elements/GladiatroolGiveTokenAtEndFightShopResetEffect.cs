using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record GladiatroolGiveTokenAtEndFightShopResetEffect : Effect
{
    public int CurrentShopResetDone { get; init; }
    public int MaxShopReset { get; init; }
    public int Quantity { get; init; }

    private GladiatroolGiveTokenAtEndFightShopResetEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int currentShopResetDone, int maxShopReset, int quantity)
        : base(id, duration, probability, criteria, effectArea)
    {
        CurrentShopResetDone = currentShopResetDone;
        MaxShopReset = maxShopReset;
        Quantity = quantity;
    }

    internal static GladiatroolGiveTokenAtEndFightShopResetEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1, (int)parameters.Param2, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, CurrentShopResetDone, MaxShopReset, Quantity);
    }
}
