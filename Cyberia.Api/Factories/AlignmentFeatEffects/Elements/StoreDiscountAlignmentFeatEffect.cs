using System.Globalization;

namespace Cyberia.Api.Factories.AlignmentFeatEffects.Elements;

public sealed record StoreDiscountAlignmentFeatEffect : AlignmentFeatEffect
{
    public int DiscountPercent { get; init; }

    public StoreDiscountAlignmentFeatEffect(int id, int discountPercent)
        : base(id)
    {
        DiscountPercent = discountPercent;
    }

    internal static StoreDiscountAlignmentFeatEffect? Create(int effectId, params ReadOnlySpan<int> parameters)
    {
        if (parameters.Length > 0)
        {
            return new(effectId, parameters[0]);
        }

        return null;
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, DiscountPercent);
    }
}
