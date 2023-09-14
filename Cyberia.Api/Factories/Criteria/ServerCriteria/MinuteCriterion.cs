namespace Cyberia.Api.Factories.Criteria.ServerCriteria
{
    public static class MinuteCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0)
            {
                return $"Minute {@operator} {values[0].Bold()}";
            }

            return null;
        }
    }
}
