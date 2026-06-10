using Cyberia.Api.Data.Items;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Interfaces;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterBoostWeaponDamagePercentEffect : Effect, IItemTypeEffect
{
    public int ItemTypeId { get; init; }
    public int PercentDamage { get; init; }

    private CharacterBoostWeaponDamagePercentEffect(int id, int itemTypeId, int percentDamage)
        : base(id)
    {
        ItemTypeId = itemTypeId;
        PercentDamage = percentDamage;
    }

    internal static CharacterBoostWeaponDamagePercentEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param1, (int)parameters.Param2);
    }

    public ItemTypeData? GetItemTypeData()
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemTypeDataById(ItemTypeId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var itemTypeName = ItemTypeId == 0
            ? Translation.Get<ApiTranslations>("Weapons", culture)
            : DofusApi.Datacenter.ItemsRepository.GetItemTypeNameById(ItemTypeId, culture);

        return GetDescription(culture, itemTypeName, PercentDamage);
    }
}
