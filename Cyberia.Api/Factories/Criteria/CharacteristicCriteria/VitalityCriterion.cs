namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria
{
    public static class VitalityCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0)
                return $"Vitalité {@operator} {values[0].Bold()}";

            return null;
        }
    }
}
