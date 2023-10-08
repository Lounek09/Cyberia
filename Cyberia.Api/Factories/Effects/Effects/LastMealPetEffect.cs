using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record LastMealPetEffect : Effect, IEffect<LastMealPetEffect>
    {
        public int ItemId { get; init; }

        private LastMealPetEffect(int effectId, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea, int itemId) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            ItemId = itemId;
        }

        public static LastMealPetEffect Create(int effectId, EffectParameters parameters, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
        }

        public ItemData? GetItemData()
        {
            return DofusApi.Instance.Datacenter.ItemsData.GetItemDataById(ItemId);
        }

        public Description GetDescription()
        {
            string itemName = ItemId == 0 ? Resources.LastMeal_None : DofusApi.Instance.Datacenter.ItemsData.GetItemNameById(ItemId);

            return GetDescription(itemName);
        }
    }
}
