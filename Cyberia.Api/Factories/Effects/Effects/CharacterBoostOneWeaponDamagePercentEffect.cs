using Cyberia.Api.Data.Items;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterBoostOneWeaponDamagePercentEffect : Effect, IEffect<CharacterBoostOneWeaponDamagePercentEffect>
{
    public int ItemTypeId { get; init; }
    public int PercentDamage { get; init; }

    private CharacterBoostOneWeaponDamagePercentEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int itemTypeId, int percentDamage)
        : base(id, duration, probability, criteria, effectArea)
    {
        ItemTypeId = itemTypeId;
        PercentDamage = percentDamage;
    }

    public static CharacterBoostOneWeaponDamagePercentEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param2);
    }

    public ItemTypeData? GetItemTypeData()
    {
        return DofusApi.Datacenter.ItemsData.GetItemTypeDataById(ItemTypeId);
    }

    public Description GetDescription()
    {
        var itemTypeName = DofusApi.Datacenter.ItemsData.GetItemTypeNameById(ItemTypeId);

        return GetDescription(itemTypeName, PercentDamage);
    }
}
