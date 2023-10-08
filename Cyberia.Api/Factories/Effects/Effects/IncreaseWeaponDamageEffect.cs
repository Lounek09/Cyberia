using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record IncreaseWeaponDamageEffect : Effect, IEffect<IncreaseWeaponDamageEffect>
    {
        public int ItemTypeId { get; init; }
        public int Value { get; init; }

        private IncreaseWeaponDamageEffect(int effectId, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea, int itemTypeId, int value) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            ItemTypeId = itemTypeId;
            Value = value;
        }

        public static IncreaseWeaponDamageEffect Create(int effectId, EffectParameters parameters, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param2);
        }

        public ItemTypeData? GetItemTypeData()
        {
            return DofusApi.Instance.Datacenter.ItemsData.GetItemTypeDataById(ItemTypeId);
        }

        public Description GetDescription()
        {
            string itemTypeName = DofusApi.Instance.Datacenter.ItemsData.GetItemTypeNameById(ItemTypeId);

            return GetDescription(itemTypeName, Value);
        }
    }
}
