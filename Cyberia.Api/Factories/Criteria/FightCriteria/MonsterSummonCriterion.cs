﻿namespace Cyberia.Api.Factories.Criteria.QuestCriteria
{
    public static class MonsterSummonCriterion
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
                        return $"{value} est invoqué dans l'équipe alliée";
                    case '=':
                        return $"{value} n'est pas invoqué dans l'équipe alliée";
                    default:
                        return value;
                }
            }

            return null;
        }
    }
}