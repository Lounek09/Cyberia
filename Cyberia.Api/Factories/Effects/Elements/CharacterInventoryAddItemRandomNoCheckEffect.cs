using Cyberia.Api.Data.Items;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterInventoryAddItemRandomNoCheckEffect : Effect
{
    public int ItemId { get; init; }
    public int Quantity { get; init; }

    private CharacterInventoryAddItemRandomNoCheckEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int itemId, int quantity)
        : base(id, duration, probability, criteria, effectArea)
    {
        ItemId = itemId;
        Quantity = quantity;
    }

    internal static CharacterInventoryAddItemRandomNoCheckEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3, (int)parameters.Param2);
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
