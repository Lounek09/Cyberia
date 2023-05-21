using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public sealed record MultiFightMonsterQuestObjective(QuestObjective QuestObjective, int MonsterId, int Quantity) :
        BasicQuestObjective(QuestObjective)
    {
        public static new MultiFightMonsterQuestObjective? Create(QuestObjective questObjective)
        {
            if (questObjective.Parameters.Count > 1 &&
                int.TryParse(questObjective.Parameters[0], out int monsterId) &&
                int.TryParse(questObjective.Parameters[1], out int quantity))
                return new(questObjective, monsterId, quantity);

            return null;
        }

        public Monster? GetMonster()
        {
            return DofusApi.Instance.Datacenter.MonstersData.GetMonsterById(MonsterId);
        }

        public override string GetDescription()
        {
            string monsterName = DofusApi.Instance.Datacenter.MonstersData.GetMonsterNameById(MonsterId);

            return GetDescriptionFromParameters(monsterName, Quantity.ToString());
        }
    }
}