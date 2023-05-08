namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public static class FreeWeightCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0)
                return $"Pods libre {@operator} {values[0].Bold()}";

            return null;
        }
    }
}
