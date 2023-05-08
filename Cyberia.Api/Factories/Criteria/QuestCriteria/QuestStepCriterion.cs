namespace Cyberia.Api.Factories.Criteria.QuestCriteria
{
    public static class QuestStepCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0 && int.TryParse(values[0], out int questStepId))
            {
                string questStepName = DofusApi.Instance.Datacenter.QuestsData.GetQuestStepNameById(questStepId);

                string value = $"Étape de quête {questStepName.Bold()}";
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