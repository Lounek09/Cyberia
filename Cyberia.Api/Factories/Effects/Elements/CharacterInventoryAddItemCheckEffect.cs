using Cyberia.Api.Data.Items;
using Cyberia.Api.Enums;
using Cyberia.Api.Extensions;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterInventoryAddItemCheckEffect : Effect
{
    public int ItemId { get; init; }
    public int Quantity { get; init; }
    public GiveItemTarget Target { get; init; }

    private CharacterInventoryAddItemCheckEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, int itemId, int quantity, GiveItemTarget giveItemTarget)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        ItemId = itemId;
        Quantity = quantity;
        Target = giveItemTarget;
    }

    internal static CharacterInventoryAddItemCheckEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, dispellable, effectArea, (int)parameters.Param3, (int)parameters.Param2, (GiveItemTarget)parameters.Param1);
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
