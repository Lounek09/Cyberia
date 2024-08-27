using Cyberia.Api.Data.Items;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterBoostWeaponDamagePercentEffect : Effect
{
    public int ItemTypeId { get; init; }
    public int PercentDamage { get; init; }

    private CharacterBoostWeaponDamagePercentEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int itemTypeId, int percentDamage)
        : base(id, duration, probability, criteria, effectArea)
    {
        ItemTypeId = itemTypeId;
        PercentDamage = percentDamage;
    }

    internal static CharacterBoostWeaponDamagePercentEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1, (int)parameters.Param2);
    }

    public ItemTypeData? GetItemTypeData()
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemTypeDataById(ItemTypeId);
    }

    public override DescriptionString GetDescription()
    {
        var itemTypeName = ItemTypeId == 0
            ? ApiTranslations.Weapons
            : DofusApi.Datacenter.ItemsRepository.GetItemTypeNameById(ItemTypeId);

        return GetDescription(itemTypeName, PercentDamage);
    }
}
