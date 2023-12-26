using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterInventoryAddItemFromRandomDropEffect : Effect, IEffect<CharacterInventoryAddItemFromRandomDropEffect>
{
    public int Quantity { get; init; }
    public int BundleId { get; init; }

    private CharacterInventoryAddItemFromRandomDropEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int quantity, int bundleId)
        : base(id, duration, probability, criteria, effectArea)
    {
        Quantity = quantity;
        BundleId = bundleId;
    }

    public static CharacterInventoryAddItemFromRandomDropEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(Quantity, null, BundleId);
    }
}
