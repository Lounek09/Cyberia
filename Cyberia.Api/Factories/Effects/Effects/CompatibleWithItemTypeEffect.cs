﻿using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record CompatibleWithItemTypeEffect : Effect, IEffect<CompatibleWithItemTypeEffect>
    {
        public int ItemTypeId { get; init; }

        private CompatibleWithItemTypeEffect(int effectId, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea, int itemTypeId) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            ItemTypeId = itemTypeId;
        }

        public static CompatibleWithItemTypeEffect Create(int effectId, EffectParameters parameters, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
        }

        public ItemTypeData? GetItemTypeData()
        {
            return DofusApi.Instance.Datacenter.ItemsData.GetItemTypeDataById(ItemTypeId);
        }

        public Description GetDescription()
        {
            string itemTypeName = DofusApi.Instance.Datacenter.ItemsData.GetItemTypeNameById(ItemTypeId);

            return GetDescription(null, null, itemTypeName);
        }
    }
}