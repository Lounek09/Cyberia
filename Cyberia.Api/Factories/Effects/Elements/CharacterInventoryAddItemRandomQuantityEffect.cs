using Cyberia.Api.Data.Items;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterInventoryAddItemRandomQuantityEffect : Effect
{
    public int ItemId { get; init; }
    public int MinQuantity { get; init; }
    public int MaxQuantity { get; init; }


    private CharacterInventoryAddItemRandomQuantityEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, int itemId, int minQuantity, int maxQuantity)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        ItemId = itemId;
        MinQuantity = minQuantity;
        MaxQuantity = maxQuantity;
    }

    internal static CharacterInventoryAddItemRandomQuantityEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, dispellable, effectArea, (int)parameters.Param3, (int)parameters.Param1, (int)parameters.Param2);
    }

    public ItemData? GetItemData()
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemDataById(ItemId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var itemName = DofusApi.Datacenter.ItemsRepository.GetItemNameById(ItemId, culture);

        return GetDescription(culture, MinQuantity, MaxQuantity, itemName);
    }
}
