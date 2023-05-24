namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria
{
    public static class AvailableSummonCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0)
                return $"Créature invocable disponible {@operator} {values[0].Bold()}";

            return null;
        }
    }
}
