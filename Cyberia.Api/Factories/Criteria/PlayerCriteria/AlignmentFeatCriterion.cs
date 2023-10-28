using Cyberia.Api.Data;

namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public sealed record AlignmentFeatCriterion : Criterion, ICriterion<AlignmentFeatCriterion>
    {
        public int AlignmentFeatId { get; init; }
        public int? Level { get; init; }

        private AlignmentFeatCriterion(string id, char @operator, int alignmentFeatId, int? level) :
            base(id, @operator)
        {
            AlignmentFeatId = alignmentFeatId;
            Level = level;
        }

        public static AlignmentFeatCriterion? Create(string id, char @operator, params string[] parameters)
        {
            if (parameters.Length > 1 && int.TryParse(parameters[0], out int alignmentFeatId) && int.TryParse(parameters[1], out int level))
            {
                return new(id, @operator, alignmentFeatId, level);
            }

            if (parameters.Length > 0 && int.TryParse(parameters[0], out alignmentFeatId))
            {
                return new(id, @operator, alignmentFeatId, null);
            }

            return null;
        }

        public AlignmentFeatData? GetAlignmentFeatData()
        {
            return DofusApi.Instance.Datacenter.AlignmentsData.GetAlignmentFeatDataById(AlignmentFeatId);
        }

        protected override string GetDescriptionName()
        {
            if (Level.HasValue)
            {
                return $"Criterion.AlignmentFeat.{GetOperatorDescriptionName()}.Level";
            }

            return $"Criterion.AlignmentFeat.{GetOperatorDescriptionName()}";
        }

        public Description GetDescription()
        {
            string alignmentName = DofusApi.Instance.Datacenter.AlignmentsData.GetAlignmentFeatNameById(AlignmentFeatId);

            if (Level.HasValue)
            {
                return GetDescription(alignmentName, Level.Value);
            }

            return GetDescription(alignmentName);
        }
    }
}
