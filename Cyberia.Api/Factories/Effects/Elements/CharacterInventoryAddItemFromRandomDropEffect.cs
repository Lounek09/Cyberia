using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterInventoryAddItemFromRandomDropEffect : Effect
{
    public int Quantity { get; init; }
    public int BundleId { get; init; }

    private CharacterInventoryAddItemFromRandomDropEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int quantity, int bundleId)
        : base(id, duration, probability, criteria, effectArea)
    {
        Quantity = quantity;
        BundleId = bundleId;
    }

    internal static CharacterInventoryAddItemFromRandomDropEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Quantity, string.Empty, BundleId);
    }
}
