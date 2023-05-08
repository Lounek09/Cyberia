namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public static class MariedCriterion
    {
        public static string? GetValue(char _, string[] values)
        {
            if (values.Length > 0)
                return values[0].Equals("0") ? "Être célibataire" : "Être marié";

            return null;
        }
    }
}
