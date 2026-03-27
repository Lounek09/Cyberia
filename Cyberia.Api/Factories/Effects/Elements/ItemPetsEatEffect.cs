using Cyberia.Api.Data.Items;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Interfaces;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record ItemPetsEatEffect : Effect, IItemEffect
{
    public int ItemId { get; init; }

    private ItemPetsEatEffect(int id, int itemId)
        : base(id)
    {
        ItemId = itemId;
    }

    internal static ItemPetsEatEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public ItemData? GetItemData()
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemDataById(ItemId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var itemName = ItemId == 0
            ? Translation.Get<ApiTranslations>("LastMeal.None", culture)
            : DofusApi.Datacenter.ItemsRepository.GetItemNameById(ItemId, culture);

        return GetDescription(culture, itemName);
    }
}
