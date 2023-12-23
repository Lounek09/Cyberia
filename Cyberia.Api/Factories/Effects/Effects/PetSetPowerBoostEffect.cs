using Cyberia.Api.Data.Items;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record PetSetPowerBoostEffect : Effect, IEffect<PetSetPowerBoostEffect>
{
    public int StatsWeightBonus { get; init; }
    public int ItemId { get; init; }

    private PetSetPowerBoostEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int statsWeightBonus, int itemId)
        : base(id, duration, probability, criteria, effectArea)
    {
        StatsWeightBonus = statsWeightBonus;
        ItemId = itemId;
    }

    public static PetSetPowerBoostEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param2, parameters.Param3);
    }

    public ItemData? GetItemData()
    {
        return DofusApi.Datacenter.ItemsData.GetItemDataById(ItemId);
    }

    public Description GetDescription()
    {
        var itemName = DofusApi.Datacenter.ItemsData.GetItemNameById(ItemId);

        return GetDescription(null, StatsWeightBonus, itemName);
    }
}
