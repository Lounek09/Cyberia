namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria
{
    public static class CurrentActionPointsCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0)
                return $"PA {@operator} {values[0].Bold()}";

            return null;
        }
    }
}
