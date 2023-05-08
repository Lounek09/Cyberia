namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public static class SexCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0)
                return $"Sexe {@operator} {(values[0].Equals("0") ? "Homme" : "Femme").Bold()}";

            return null;
        }
    }
}
