namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public static class PlayerRightsCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0)
                return $"Niveau admin {@operator} {values[0].Bold()}";

            return null;
        }
    }
}
