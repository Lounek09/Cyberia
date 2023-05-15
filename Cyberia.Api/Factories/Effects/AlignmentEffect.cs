using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed class AlignmentEffect : BasicEffect
    {
        public int AlignmentId { get; init; }

        public AlignmentEffect(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area) : 
            base(effectId, parameters, duration, probability, criteria, area)
        {
            AlignmentId = parameters.Param3;
        }

        public static new AlignmentEffect Create(int effectId, EffectParameters parameters, int duration, int probability, string criteria, Area area)
        {
            return new(effectId, parameters, duration, probability, criteria, area);
        }

        public Alignment? GetAlignment()
        {
            return DofusApi.Instance.Datacenter.AlignmentsData.GetAlignmentById(AlignmentId);
        }

        public override string GetDescription()
        {
            string alignmentName = DofusApi.Instance.Datacenter.AlignmentsData.GetAlignmentNameById(AlignmentId);

            return GetDescriptionFromParameters(null, null, alignmentName);
        }
    }
}
