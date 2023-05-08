namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public static class AlignmentSpecializationCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0 && int.TryParse(values[0], out int alignmentSpecializationId))
            {
                string alignmentSpecializationName = DofusApi.Instance.Datacenter.AlignmentsData.GetAlignmentSpecializationNameById(alignmentSpecializationId);

                return $"Spécialisation {@operator} {alignmentSpecializationName.Bold()}";
            }

            return null;
        }
    }
}
