namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria
{
    public static class AgilityCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0)
                return $"Agilité {@operator} {values[0].Bold()}";

            return null;
        }
    }
}
