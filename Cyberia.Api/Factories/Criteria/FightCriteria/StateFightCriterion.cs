namespace Cyberia.Api.Factories.Criteria.QuestCriteria
{
    public static class StateFightCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0 && int.TryParse(values[0], out int stateId))
            {
                string stateName = DofusApi.Instance.Datacenter.StatesData.GetStateNameById(stateId);

                switch (@operator)
                {
                    case '≠':
                        return $"Etre dans l'état {stateName}";
                    case '=':
                        return $"Ne pas être dans l'état {stateName}";
                }
            }

            return null;
        }
    }
}
