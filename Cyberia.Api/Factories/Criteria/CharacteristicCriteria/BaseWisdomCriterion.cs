namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria
{
    public static class BaseWisdomCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0)
                return $"Sagesse de base {@operator} {values[0].Bold()}";

            return null;
        }
    }
}
