namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public static class AlignmentCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0 && int.TryParse(values[0], out int alignmentId))
            {
                string alignementName = DofusApi.Instance.Datacenter.AlignmentsData.GetAlignmentNameById(alignmentId);

                return $"Alignement {@operator} {alignementName.Bold()}";
            }

            return null;
        }
    }
}
