using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed class LastMealPetEffect : BasicEffect
    {
        public int ItemId { get; init; }

        public LastMealPetEffect(int effectId, EffectParameters parameters, int duration, int probability, Area area) : 
            base(effectId, parameters, duration, probability, area)
        {
            ItemId = parameters.Param3;
        }

        public static new LastMealPetEffect Create(int effectId, EffectParameters parameters, int duration, int probability, Area area)
        {
            return new(effectId, parameters, duration, probability, area);
        }

        public Item? GetItem()
        {
            return DofusApi.Instance.Datacenter.ItemsData.GetItemById(ItemId);
        }

        public override string GetDescription()
        {
            string itemName = ItemId == 0 ? "Aucun" : DofusApi.Instance.Datacenter.ItemsData.GetItemNameById(ItemId).SanitizeMarkDown();

            return GetDescriptionFromParameters(itemName);
        }
    }
}
