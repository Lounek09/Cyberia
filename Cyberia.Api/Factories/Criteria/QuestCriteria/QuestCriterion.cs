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
                return @operator switch
                {
                    '≠' => $"{value} pas en cours",
                    '=' => $"{value} en cours",
                    '>' => $"{value} validée",
                    '<' => $"{value} non validée",
                    _ => value,
                };
            }

            return null;
        }
    }
}
