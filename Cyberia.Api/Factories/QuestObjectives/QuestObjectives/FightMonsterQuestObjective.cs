using Cyberia.Api.Data;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public sealed record FightMonsterQuestObjective : QuestObjective, IQuestObjective<FightMonsterQuestObjective>
    {
        public int MonsterId { get; init; }

        private FightMonsterQuestObjective(QuestObjectiveData questObjectiveData, int monsterId) :
            base(questObjectiveData)
        {
            MonsterId = monsterId;
        }

        public static FightMonsterQuestObjective? Create(QuestObjectiveData questObjectiveData)
        {
            List<string> parameters = questObjectiveData.Parameters;
            if (parameters.Count > 0 && int.TryParse(parameters[0], out int monsterId))
            {
                return new(questObjectiveData, monsterId);
            }

            return null;
        }

        public MonsterData? GetMonsterData()
        {
            return DofusApi.Instance.Datacenter.MonstersData.GetMonsterDataById(MonsterId);
        }

        public Description GetDescription()
        {
            string monsterName = DofusApi.Instance.Datacenter.MonstersData.GetMonsterNameById(MonsterId);

            return GetDescription(monsterName);
        }
    }
}
