namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria
{
    public static class BaseIntelligenceCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0)
                return $"Intelligence de base {@operator} {values[0].Bold()}";

            return null;
        }
    }
}
