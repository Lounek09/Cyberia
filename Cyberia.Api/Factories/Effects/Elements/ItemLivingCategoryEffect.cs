using Cyberia.Api.Data.Items;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Interfaces;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record ItemLivingCategoryEffect : Effect, IItemTypeEffect
{
    public int ItemTypeId { get; init; }

    private ItemLivingCategoryEffect(int id, int itemTypeId)
        : base(id)
    {
        ItemTypeId = itemTypeId;
    }

    internal static ItemLivingCategoryEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public ItemTypeData? GetItemTypeData()
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemTypeDataById(ItemTypeId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var itemTypeName = DofusApi.Datacenter.ItemsRepository.GetItemTypeNameById(ItemTypeId, culture);

        return GetDescription(culture, string.Empty, string.Empty, itemTypeName);
    }
}
