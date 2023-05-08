namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public static class LookCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0)
                return $"Apparence {@operator} {values[0].Bold()}";

            return null;
        }
    }
}
