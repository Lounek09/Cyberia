namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria
{
    public static class DisgracePointsCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0)
                return $"Points de déshonneur {@operator} {values[0].Bold()}";

            return null;
        }
    }
}
