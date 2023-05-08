namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria
{
    public static class WisdomCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0)
                return $"Sagesse {@operator} {values[0].Bold()}";

            return null;
        }
    }
}
