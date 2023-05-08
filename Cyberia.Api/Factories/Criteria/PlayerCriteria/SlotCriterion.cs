namespace Cyberia.Api.Factories.Criteria.PlayerCriteria
{
    public static class SlotCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0)
            {
                string value = $"Emplacement {values[0].Bold()}";
                switch (@operator)
                {
                    case '≠':
                        return $"{value} libre";
                    case '=':
                        return $"{value} occupé";
                    default:
                        return value;
                }
            }

            return null;
        }
    }
}
