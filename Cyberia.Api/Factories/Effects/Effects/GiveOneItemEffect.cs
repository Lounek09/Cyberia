﻿using Cyberia.Api.Data;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;
using Cyberia.Api.Values;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record GiveOneItemEffect : Effect, IEffect<GiveOneItemEffect>
    {
        public int ItemId { get; init; }

        private GiveOneItemEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int itemId) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            ItemId = itemId;
        }

        public static GiveOneItemEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, parameters.Param3); ;
        }

        public ItemData? GetItemData()
        {
            return DofusApi.Datacenter.ItemsData.GetItemDataById(ItemId);
        }

        public Description GetDescription()
        {
            string itemName = DofusApi.Datacenter.ItemsData.GetItemNameById(ItemId);

            return GetDescription(null, null, itemName);
        }
    }
}