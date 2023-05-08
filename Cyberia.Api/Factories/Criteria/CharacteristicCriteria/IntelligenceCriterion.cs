namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria
{
    public static class IntelligenceCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0)
                return $"Intelligence {@operator} {values[0].Bold()}";

            return null;
        }
    }
}
