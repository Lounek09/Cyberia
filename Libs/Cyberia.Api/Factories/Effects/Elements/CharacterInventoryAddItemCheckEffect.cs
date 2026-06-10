using Cyberia.Api.Data.Items;
using Cyberia.Api.Enums;
using Cyberia.Api.Extensions;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Interfaces;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterInventoryAddItemCheckEffect : Effect, IItemEffect
{
    public int ItemId { get; init; }
    public int Quantity { get; init; }
    public GiveItemTarget Target { get; init; }

    private CharacterInventoryAddItemCheckEffect(int id, int itemId, int quantity, GiveItemTarget giveItemTarget)
        : base(id)
    {
        ItemId = itemId;
        Quantity = quantity;
        Target = giveItemTarget;
    }

    internal static CharacterInventoryAddItemCheckEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3, (int)parameters.Param2, (GiveItemTarget)parameters.Param1);
    }

    public ItemData? GetItemData()
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemDataById(ItemId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var itemName = DofusApi.Datacenter.ItemsRepository.GetItemNameById(ItemId, culture);

        return GetDescription(culture, Target.GetDescription(culture), Quantity, itemName);
    }
}
