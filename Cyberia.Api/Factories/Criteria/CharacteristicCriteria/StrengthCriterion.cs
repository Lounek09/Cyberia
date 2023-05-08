namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria
{
    public static class StrengthCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0)
                return $"Force {@operator} {values[0].Bold()}";

            return null;
        }
    }
}