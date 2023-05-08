namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public static class AlignmentFeatCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0 && int.TryParse(values[0], out int alignmentFeatId))
            {
                string alignementFeatName = DofusApi.Instance.Datacenter.AlignmentsData.GetAlignmentFeatNameById(alignmentFeatId);

                if (values.Length > 1)
                    return $"Don {alignementFeatName.Bold()} {@operator} {values[1].Bold()}";

                return $"Don {@operator} {alignementFeatName.Bold()}";
            }

            return null;
        }
    }
}
