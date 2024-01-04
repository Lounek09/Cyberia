namespace Cyberia.Api.Factories.AlignmentFeatEffects;

public sealed record StoreDiscountAlignmentFeatEffect
    : AlignmentFeatEffect, IAlignmentFeatEffect
{
    public int DiscountPercent { get; init; }

    public StoreDiscountAlignmentFeatEffect(int id, int discountPercent)
        : base(id)
    {
        DiscountPercent = discountPercent;
    }

    internal static StoreDiscountAlignmentFeatEffect? Create(int effectId, params int[] parameters)
    {
        if (parameters.Length > 0)
        {
            return new(effectId, parameters[0]);
        }

        return null;
    }

    public Description GetDescription()
    {
        return GetDescription(DiscountPercent);
    }
}
