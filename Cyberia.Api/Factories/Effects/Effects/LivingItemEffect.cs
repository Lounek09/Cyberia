using Cyberia.Api.Data;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record LivingItemEffect : Effect, IEffect<LivingItemEffect>
    {
        public int ItemId { get; init; }

        private LivingItemEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int itemId) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            ItemId = itemId;
        }

        public static LivingItemEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
        }

        public ItemData? GetItemData()
        {
            return DofusApi.Instance.Datacenter.ItemsData.GetItemDataById(ItemId);
        }

        public Description GetDescription()
        {
            string itemName = DofusApi.Instance.Datacenter.ItemsData.GetItemNameById(ItemId);

            return GetDescription(null, null, itemName);
        }
    }
}
