using Cyberia.Api.Data.Items;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterInventoryAddItemRandomQuantityEffect : Effect, IEffect
{
    public int ItemId { get; init; }
    public int MinQuantity { get; init; }
    public int MaxQuantity { get; init; }


    private CharacterInventoryAddItemRandomQuantityEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int itemId, int minQuantity, int maxQuantity)
        : base(id, duration, probability, criteria, effectArea)
    {
        ItemId = itemId;
        MinQuantity = minQuantity;
        MaxQuantity = maxQuantity;
    }

    internal static CharacterInventoryAddItemRandomQuantityEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3, (int)parameters.Param1, (int)parameters.Param2);
    }

    public ItemData? GetItemData()
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemDataById(ItemId);
    }

    public Description GetDescription()
    {
        var itemName = DofusApi.Datacenter.ItemsRepository.GetItemNameById(ItemId);

        return GetDescription(MinQuantity, MaxQuantity, itemName);
    }
}
