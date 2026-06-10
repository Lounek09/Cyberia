using Cyberia.Api.Data.Items;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Interfaces;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterInventoryAddItemEffect : Effect, IItemEffect
{
    public int ItemId { get; init; }
    public int Quantity { get; init; }

    private CharacterInventoryAddItemEffect(int id, int itemId, int quantity)
        : base(id)
    {
        ItemId = itemId;
        Quantity = quantity;
    }

    internal static CharacterInventoryAddItemEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3, (int)parameters.Param2);
    }

    public ItemData? GetItemData()
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemDataById(ItemId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var itemName = DofusApi.Datacenter.ItemsRepository.GetItemNameById(ItemId, culture);

        return GetDescription(culture, string.Empty, Quantity, itemName);
    }
}
