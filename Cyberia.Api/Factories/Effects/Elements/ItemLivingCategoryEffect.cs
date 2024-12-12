using Cyberia.Api.Data.Items;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record ItemLivingCategoryEffect : Effect
{
    public int ItemTypeId { get; init; }

    private ItemLivingCategoryEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, int itemTypeId)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        ItemTypeId = itemTypeId;
    }

    internal static ItemLivingCategoryEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, dispellable, effectArea, (int)parameters.Param3);
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
