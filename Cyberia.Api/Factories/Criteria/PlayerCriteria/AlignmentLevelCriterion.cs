namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public static class AlignmentLevelCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0)
                return $"Niveau d'alignement {@operator} {values[0].Bold()}";

            return null;
        }
    }
}
