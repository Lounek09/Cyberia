namespace Cyberia.Api.Factories.Criteria.QuestCriteria
{
    public static class TurnCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0)
            {
                switch (@operator)
                {
                    case '%':
                        if (values[0].Equals("2:1"))
                            return $"Tour {"impair".Bold()}";
                        if (values[0].Equals("2:0"))
                            return $"Tour {"pair".Bold()}";
                        break;
                    default:
                        return $"Tour {@operator} {values[0].Bold()}";
                }
            }

            return null;
        }
    }
}
