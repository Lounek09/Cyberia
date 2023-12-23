using Cyberia.Api.Data.Items;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record ItemLivingCategoryEffect : Effect, IEffect<ItemLivingCategoryEffect>
{
    public int ItemTypeId { get; init; }

    private ItemLivingCategoryEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int itemTypeId)
        : base(id, duration, probability, criteria, effectArea)
    {
        ItemTypeId = itemTypeId;
    }

    public static ItemLivingCategoryEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public ItemTypeData? GetItemTypeData()
    {
        return DofusApi.Datacenter.ItemsData.GetItemTypeDataById(ItemTypeId);
    }

    public Description GetDescription()
    {
        var itemTypeName = DofusApi.Datacenter.ItemsData.GetItemTypeNameById(ItemTypeId);

        return GetDescription(null, null, itemTypeName);
    }
}
