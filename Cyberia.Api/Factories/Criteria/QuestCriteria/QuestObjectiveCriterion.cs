namespace Cyberia.Api.Factories.Criteria.QuestCriteria
{
    public static class QuestObjectiveCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0 && int.TryParse(values[0], out int questId))
            {
                string questObjectiveDescription = DofusApi.Instance.Datacenter.QuestsData.GetQuestObjectiveDescriptionById(questId);

                string value = $"Objectif de quête {questObjectiveDescription}";
                switch (@operator)
                {
                    case '≠':
                        return $"{value} non finissable";
                    case '=':
                        return $"{value} finissable";
                    case '>':
                        return $"{value} validé";
                    case '<':
                        return $"{value} non validé";
                    default:
                        return value;
                }
            }

            return null;
        }
    }
}