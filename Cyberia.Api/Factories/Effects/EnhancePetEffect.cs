using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed class EnhancePetEffect : BasicEffect
    {
        public int StatsWeightBonus { get; init; }
        public int ItemId { get; init; }

        public EnhancePetEffect(int effectId, EffectParameters parameters, int duration, int probability, Area area) : 
            base(effectId, parameters, duration, probability, area)
        {
            StatsWeightBonus = parameters.Param2;
            ItemId = parameters.Param3;
        }

        public static new EnhancePetEffect Create(int effectId, EffectParameters parameters, int duration, int probability, Area area)
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

            return GetDescriptionFromParameters(null, StatsWeightBonus.ToString(), itemName);
        }
    }
}
