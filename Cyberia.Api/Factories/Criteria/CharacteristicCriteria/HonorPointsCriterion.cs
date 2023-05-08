namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria
{
    public static class HonorPointsCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0)
                return $"Points d'honneur {@operator} {values[0].Bold()}";
            return null;
        }
    }
}
