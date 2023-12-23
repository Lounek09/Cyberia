using Cyberia.Api.Data.Items;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterInventoryAddItemRandomNoCheckEffect : Effect, IEffect<CharacterInventoryAddItemRandomNoCheckEffect>
{
    public int ItemId { get; init; }
    public int Qte { get; init; }

    private CharacterInventoryAddItemRandomNoCheckEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int itemId, int qte)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        ItemId = itemId;
        Qte = qte;
    }

    public static CharacterInventoryAddItemRandomNoCheckEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3, parameters.Param2);
    }

    public ItemData? GetItemData()
    {
        return DofusApi.Datacenter.ItemsData.GetItemDataById(ItemId);
    }

    public Description GetDescription()
    {
        var itemName = DofusApi.Datacenter.ItemsData.GetItemNameById(ItemId);

        return GetDescription(null, Qte, itemName);
    }
}
