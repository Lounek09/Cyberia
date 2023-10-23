using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public sealed record AlignmentCriterion : Criterion, ICriterion<AlignmentCriterion>
    {
        public int AlignmentId { get; init; }

        private AlignmentCriterion(string id, char @operator, int alignmentId) :
            base(id, @operator)
        {
            AlignmentId = alignmentId;
        }

        public static AlignmentCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 0 && int.TryParse(parameters[0], out int alignmentId))
            {
                return new(id, @operator, alignmentId);
            }

            return null;
        }

        public AlignmentData? GetAlignmentData()
        {
            return DofusApi.Instance.Datacenter.AlignmentsData.GetAlignmentDataById(AlignmentId);
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.Alignment.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            string alignmentName = DofusApi.Instance.Datacenter.AlignmentsData.GetAlignmentNameById(AlignmentId);

            return GetDescription(alignmentName);
        }
    }
}
