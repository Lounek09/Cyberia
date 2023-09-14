namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public static class SlotCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0)
            {
                string value = $"Emplacement {values[0].Bold()}";
                return @operator switch
                {
                    '≠' => $"{value} libre",
                    '=' => $"{value} occupé",
                    _ => value,
                };
            }

            return null;
        }
    }
}
