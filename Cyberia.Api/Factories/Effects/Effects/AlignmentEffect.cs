using Cyberia.Api.Data;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record AlignmentEffect : Effect, IEffect<AlignmentEffect>
    {
        public int AlignmentId { get; init; }

        private AlignmentEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int alignmentId) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            AlignmentId = alignmentId;
        }

        public static AlignmentEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
        }

        public AlignmentData? GetAlignmentData()
        {
            return DofusApi.Datacenter.AlignmentsData.GetAlignmentDataById(AlignmentId);
        }

        public Description GetDescription()
        {
            string alignmentName = DofusApi.Datacenter.AlignmentsData.GetAlignmentNameById(AlignmentId);

            return GetDescription(null, null, alignmentName);
        }
    }
}
