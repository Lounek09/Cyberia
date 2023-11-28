using Cyberia.Api.Data.Items;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record DeleteItemEffect : Effect, IEffect<DeleteItemEffect>
{
    public int ItemId { get; init; }

    private DeleteItemEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int itemId)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        ItemId = itemId;
    }

    public static DeleteItemEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1);
    }

    public ItemData? GetItemData()
    {
        return DofusApi.Datacenter.ItemsData.GetItemDataById(ItemId);
    }

    public Description GetDescription()
    {
        var itemName = DofusApi.Datacenter.ItemsData.GetItemNameById(ItemId);

        return GetDescription(itemName);
    }
}
