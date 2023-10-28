using Cyberia.Api.Data;

namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public sealed record AlignmentSpecializationCriterion : Criterion, ICriterion<AlignmentSpecializationCriterion>
    {
        public int AlignmentSpecializationId { get; init; }

        private AlignmentSpecializationCriterion(string id, char @operator, int alignmentSpecializationId) :
            base(id, @operator)
        {
            AlignmentSpecializationId = alignmentSpecializationId;
        }

        public static AlignmentSpecializationCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 0 && int.TryParse(parameters[0], out int alignmentSpecializationId))
            {
                return new(id, @operator, alignmentSpecializationId);
            }

            return null;
        }

        public AlignmentSpecializationData? GetAlignmentSpecializationData()
        {
            return DofusApi.Instance.Datacenter.AlignmentsData.GetAlignmentSpecializationDataById(AlignmentSpecializationId);
        }

        protected override string GetDescriptionName()
        {
            return $"Criterion.AlignmentSpecialization.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            string alignmentSpecializationName = DofusApi.Instance.Datacenter.AlignmentsData.GetAlignmentSpecializationNameById(AlignmentSpecializationId);

            return GetDescription(alignmentSpecializationName);
        }
    }
}
