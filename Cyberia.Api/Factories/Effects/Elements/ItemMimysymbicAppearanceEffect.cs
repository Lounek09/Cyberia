using Cyberia.Api.Data.Items;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record ItemMimysymbicAppearanceEffect : Effect
{
    public int ItemId { get; init; }

    private ItemMimysymbicAppearanceEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int itemId)
        : base(id, duration, probability, criteria, effectArea)
    {
        ItemId = itemId;
    }

    internal static ItemMimysymbicAppearanceEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public ItemData? GetItemData()
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemDataById(ItemId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var itemName = DofusApi.Datacenter.ItemsRepository.GetItemNameById(ItemId, culture);

        return GetDescription(culture, string.Empty, string.Empty, itemName);
    }
}
