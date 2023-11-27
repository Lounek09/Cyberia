using Cyberia.Api.Data;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record DisplayEffectsFromItemEffect : Effect, IEffect<DisplayEffectsFromItemEffect>
{
    public int ItemId { get; init; }

    private DisplayEffectsFromItemEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int itemId)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        ItemId = itemId;
    }

    public static DisplayEffectsFromItemEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public ItemData? GetItemData()
    {
        return DofusApi.Datacenter.ItemsData.GetItemDataById(ItemId);
    }

    public Description GetDescription()
    {
        var itemName = DofusApi.Datacenter.ItemsData.GetItemNameById(ItemId);

        return GetDescription(null, null, itemName);
    }
}
