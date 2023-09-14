namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria
{
    public static class CurrentMovementPointsCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0)
                return $"PM {@operator} {values[0].Bold()}";

            return null;
        }
    }
}
