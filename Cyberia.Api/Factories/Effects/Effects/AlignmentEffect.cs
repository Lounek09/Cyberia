﻿using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record AlignmentEffect : Effect, IEffect<AlignmentEffect>
    {
        public int AlignmentId { get; init; }

        private AlignmentEffect(int effectId, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea, int alignmentId) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            AlignmentId = alignmentId;
        }

        public static AlignmentEffect Create(int effectId, EffectParameters parameters, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
        }

        public AlignmentData? GetAlignmentData()
        {
            return DofusApi.Instance.Datacenter.AlignmentsData.GetAlignmentDataById(AlignmentId);
        }

        public Description GetDescription()
        {
            string alignmentName = DofusApi.Instance.Datacenter.AlignmentsData.GetAlignmentNameById(AlignmentId);

            return GetDescription(null, null, alignmentName);
        }
    }
}