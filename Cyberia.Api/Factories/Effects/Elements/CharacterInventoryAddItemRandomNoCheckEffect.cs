using Cyberia.Api.Data.Items;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterInventoryAddItemRandomNoCheckEffect : Effect, IEffect
{
    public int ItemId { get; init; }
    public int Qte { get; init; }

    private CharacterInventoryAddItemRandomNoCheckEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int itemId, int qte)
        : base(id, duration, probability, criteria, effectArea)
    {
        ItemId = itemId;
        Qte = qte;
    }

    internal static CharacterInventoryAddItemRandomNoCheckEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3, (int)parameters.Param2);
    }

    public ItemData? GetItemData()
    {
        return DofusApi.Datacenter.ItemsData.GetItemDataById(ItemId);
    }

    public Description GetDescription()
    {
        var itemName = DofusApi.Datacenter.ItemsData.GetItemNameById(ItemId);

        return GetDescription(string.Empty, Qte, itemName);
    }
}
