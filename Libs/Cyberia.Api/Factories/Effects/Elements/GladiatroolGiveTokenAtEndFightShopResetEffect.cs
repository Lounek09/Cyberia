using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record GladiatroolGiveTokenAtEndFightShopResetEffect : Effect
{
    public int CurrentShopResetDone { get; init; }
    public int MaxShopReset { get; init; }
    public int Quantity { get; init; }

    private GladiatroolGiveTokenAtEndFightShopResetEffect(int id, int currentShopResetDone, int maxShopReset, int quantity)
        : base(id)
    {
        CurrentShopResetDone = currentShopResetDone;
        MaxShopReset = maxShopReset;
        Quantity = quantity;
    }

    internal static GladiatroolGiveTokenAtEndFightShopResetEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param1, (int)parameters.Param2, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, CurrentShopResetDone, MaxShopReset, Quantity);
    }
}
