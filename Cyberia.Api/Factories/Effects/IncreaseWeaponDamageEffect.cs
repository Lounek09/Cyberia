using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed class IncreaseWeaponDamageEffect : BasicEffect
    {
        public int ItemTypeId { get; init; }
        public int Value { get; init; }

        public IncreaseWeaponDamageEffect(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area) : 
            base(effectId, parameters, duration, probability, criteria, area)
        {
            ItemTypeId = parameters.Param1;
            Value = parameters.Param2;
        }

        public static new IncreaseWeaponDamageEffect Create(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area)
        {
            return new(effectId, parameters, duration, probability, criteria, area);
        }

        public ItemType? GetItemType()
        {
            return DofusApi.Instance.Datacenter.ItemsData.GetItemTypeById(ItemTypeId);
        }

        public override string GetDescription()
        {
            string itemTypeName = DofusApi.Instance.Datacenter.ItemsData.GetItemTypeNameById(ItemTypeId);

            return GetDescriptionFromParameters(itemTypeName, Value.ToString());
        }
    }
}
