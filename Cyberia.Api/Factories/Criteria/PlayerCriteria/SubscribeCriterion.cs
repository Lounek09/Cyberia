namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public static class SubscribeCriterion
    {
        public static string? GetValue(char _, string[] values)
        {
            if (values.Length > 0)
                return (values[0].Equals("0") ? "Ne pas être abonné" : "Être abonné");

            return null;
        }
    }
}
