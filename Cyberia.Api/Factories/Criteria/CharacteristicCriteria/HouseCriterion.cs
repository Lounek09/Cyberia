namespace Cyberia.Api.Factories.Criteria.CharacteristicCriteria
{
    public static class HouseCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0 && values[0].Equals("1") && @operator is '=')
                return "Etre propriétaire d'une maison";

            return null;
        }
    }
}
