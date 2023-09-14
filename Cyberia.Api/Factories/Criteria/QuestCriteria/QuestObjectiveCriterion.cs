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
                return @operator switch
                {
                    '≠' => $"{value} non finissable",
                    '=' => $"{value} finissable",
                    '>' => $"{value} validé",
                    '<' => $"{value} non validé",
                    _ => value,
                };
            }

            return null;
        }
    }
}
