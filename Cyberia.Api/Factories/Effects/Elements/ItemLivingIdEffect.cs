using Cyberia.Api.Data.Items;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record ItemLivingIdEffect
    : Effect, IEffect
{
    public int ItemId { get; init; }

    private ItemLivingIdEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int itemId)
        : base(id, duration, probability, criteria, effectArea)
    {
        ItemId = itemId;
    }

    internal static ItemLivingIdEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public ItemData? GetItemData()
    {
        return DofusApi.Datacenter.ItemsData.GetItemDataById(ItemId);
    }

    public Description GetDescription()
    {
        var itemName = DofusApi.Datacenter.ItemsData.GetItemNameById(ItemId);

        return GetDescription(string.Empty, string.Empty, itemName);
    }
}
