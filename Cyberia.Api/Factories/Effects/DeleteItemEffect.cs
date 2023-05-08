using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed class DeleteItemEffect : BasicEffect
    {
        public int ItemId { get; init; }

        public DeleteItemEffect(int effectId, EffectParameters parameters, int duration, int probability, Area area) : 
            base(effectId, parameters, duration, probability, area)
        {
            ItemId = parameters.Param1;
        }

        public static new DeleteItemEffect Create(int effectId, EffectParameters parameters, int duration, int probability, Area area)
        {
            return new(effectId, parameters, duration, probability, area);
        }

        public Item? GetItem()
        {
            return DofusApi.Instance.Datacenter.ItemsData.GetItemById(ItemId);
        }

        public override string GetDescription()
        {
            string itemName = DofusApi.Instance.Datacenter.ItemsData.GetItemNameById(ItemId).SanitizeMarkDown();

            return GetDescriptionFromParameters(itemName);
        }
    }
}
