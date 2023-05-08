namespace Cyberia.Api.Factories.Criteria.MapCriteria
{
    public static class MapPlayerCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0)
                return $"Nombre de joueurs sur la map {values[0].Bold()} {@operator} {(values.Length > 1 ? values[1] : "?").Bold()}";

            return null;
        }
    }
}
