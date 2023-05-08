namespace Cyberia.Api.Factories.Criteria.QuestCriteria
{
    public static class QuestCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0 && int.TryParse(values[0], out int questId))
            {
                string questName = DofusApi.Instance.Datacenter.QuestsData.GetQuestNameById(questId);

                string value = $"Quête {questName.Bold()}";
                switch (@operator)
                {
                    case '≠':
                        return $"{value} pas en cours";
                    case '=':
                        return $"{value} en cours";
                    case '>':
                        return $"{value} validée";
                    case '<':
                        return $"{value} non validée";
                    default:
                        return value;
                }
            }

            return null;
        }
    }
}
