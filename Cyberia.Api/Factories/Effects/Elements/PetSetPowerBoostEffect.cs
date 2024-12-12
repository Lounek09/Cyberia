using Cyberia.Api.Data.Items;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record PetSetPowerBoostEffect : Effect
{
    public int StatsWeightBonus { get; init; }
    public int ItemId { get; init; }

    private PetSetPowerBoostEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, int statsWeightBonus, int itemId)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        StatsWeightBonus = statsWeightBonus;
        ItemId = itemId;
    }

    internal static PetSetPowerBoostEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, dispellable, effectArea, (int)parameters.Param2, (int)parameters.Param3);
    }

    public ItemData? GetItemData()
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemDataById(ItemId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var itemName = DofusApi.Datacenter.ItemsRepository.GetItemNameById(ItemId, culture);

        return GetDescription(culture, string.Empty, StatsWeightBonus, itemName);
    }
}
