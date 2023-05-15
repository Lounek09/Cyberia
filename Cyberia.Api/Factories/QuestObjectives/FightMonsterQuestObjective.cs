using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public sealed class FightMonsterQuestObjective : BasicQuestObjective
    {
        public int MonsterId { get; init; }

        public FightMonsterQuestObjective(QuestObjective questObjective, int monsterId) :
            base(questObjective)
        {
            MonsterId = monsterId;
        }

        public static new FightMonsterQuestObjective? Create(QuestObjective questObjective)
        {
            if (questObjective.Parameters.Count > 0 &&
                int.TryParse(questObjective.Parameters[0], out int monsterId))
                return new(questObjective, monsterId);

            return null;
        }

        public Monster? GetMonster()
        {
            return DofusApi.Instance.Datacenter.MonstersData.GetMonsterById(MonsterId);
        }

        public override string GetDescription()
        {
            string monsterName = DofusApi.Instance.Datacenter.MonstersData.GetMonsterNameById(MonsterId);

            return GetDescriptionFromParameters(monsterName);
        }
    }
}
