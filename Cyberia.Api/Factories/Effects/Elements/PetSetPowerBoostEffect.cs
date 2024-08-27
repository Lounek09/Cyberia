using Cyberia.Api.Data.Items;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record PetSetPowerBoostEffect : Effect
{
    public int StatsWeightBonus { get; init; }
    public int ItemId { get; init; }

    private PetSetPowerBoostEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int statsWeightBonus, int itemId)
        : base(id, duration, probability, criteria, effectArea)
    {
        StatsWeightBonus = statsWeightBonus;
        ItemId = itemId;
    }

    internal static PetSetPowerBoostEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param2, (int)parameters.Param3);
    }

    public ItemData? GetItemData()
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemDataById(ItemId);
    }

    public override DescriptionString GetDescription()
    {
        var itemName = DofusApi.Datacenter.ItemsRepository.GetItemNameById(ItemId);

        return GetDescription(string.Empty, StatsWeightBonus, itemName);
    }
}
