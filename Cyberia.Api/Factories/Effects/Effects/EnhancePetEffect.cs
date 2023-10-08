using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record EnhancePetEffect : Effect, IEffect<EnhancePetEffect>
    {
        public int StatsWeightBonus { get; init; }
        public int ItemId { get; init; }

        private EnhancePetEffect(int effectId, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea, int statsWeightBonus, int itemId) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            StatsWeightBonus = statsWeightBonus;
            ItemId = itemId;
        }

        public static EnhancePetEffect Create(int effectId, EffectParameters parameters, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, parameters.Param2, parameters.Param3);
        }

        public ItemData? GetItemData()
        {
            return DofusApi.Instance.Datacenter.ItemsData.GetItemDataById(ItemId);
        }

        public Description GetDescription()
        {
            string itemName = DofusApi.Instance.Datacenter.ItemsData.GetItemNameById(ItemId);

            return GetDescription(null, StatsWeightBonus, itemName);
        }
    }
}
