namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public static class KamasCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0)
                return $"Kamas {@operator} {values[0].Bold()}";

            return null;
        }
    }
}
