using Cyberia.Api.Data.Items;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Interfaces;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record PetSetPowerBoostEffect : Effect, IItemEffect
{
    public int StatsWeightBonus { get; init; }
    public int ItemId { get; init; }

    private PetSetPowerBoostEffect(int id, int statsWeightBonus, int itemId)
        : base(id)
    {
        StatsWeightBonus = statsWeightBonus;
        ItemId = itemId;
    }

    internal static PetSetPowerBoostEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param2, (int)parameters.Param3);
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
