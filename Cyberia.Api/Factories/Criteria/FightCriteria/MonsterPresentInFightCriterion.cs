namespace Cyberia.Api.Factories.Criteria.QuestCriteria
{
    public static class MonsterPresentInFightCriterion
    {
        public static string? GetValue(char @operator, string[] values)
        {
            if (values.Length > 0 && int.TryParse(values[0], out int monsterId))
            {
                string monsterName = DofusApi.Instance.Datacenter.MonstersData.GetMonsterNameById(monsterId);

                string value = $"Le monstre {monsterName.Bold()}";
                switch (@operator)
                {
                    case '≠':
                        return $"{value} est présent dans le combat";
                    case '=':
                        return $"{value} n'est pas présent dans le combat";
                    default:
                        return value;
                }
            }

            return null;
        }
    }
}
