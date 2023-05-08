namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria
{
    public static class BaseVitalityCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0)
                return $"Vitalité de base {@operator} {values[0].Bold()}";

            return null;
        }
    }
}
