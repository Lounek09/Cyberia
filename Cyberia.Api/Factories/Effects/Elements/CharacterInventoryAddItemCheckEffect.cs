using Cyberia.Api.Data.Items;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;
using Cyberia.Api.Values;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterInventoryAddItemCheckEffect : Effect, IEffect
{
    public int ItemId { get; init; }
    public int Qte { get; init; }
    public GiveItemTarget Target { get; init; }

    private CharacterInventoryAddItemCheckEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int itemId, int qte, GiveItemTarget giveItemTarget)
        : base(id, duration, probability, criteria, effectArea)
    {
        ItemId = itemId;
        Qte = qte;
        Target = giveItemTarget;
    }

    internal static CharacterInventoryAddItemCheckEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3, (int)parameters.Param2, (GiveItemTarget)parameters.Param1);
    }

    public ItemData? GetItemData()
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemDataById(ItemId);
    }

    public Description GetDescription()
    {
        var itemName = DofusApi.Datacenter.ItemsRepository.GetItemNameById(ItemId);

        return GetDescription(Target.GetDescription(), Qte, itemName);
    }
}
