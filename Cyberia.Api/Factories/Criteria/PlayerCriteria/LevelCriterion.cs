namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public static class LevelCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0)
                return $"Niveau {@operator} {values[0].Bold()}";

            return null;
        }
    }
}
