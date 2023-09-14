using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public sealed record MultiFightMonsterQuestObjective : BasicQuestObjective
    {
        public int MonsterId { get; init; }
        public int Quantity { get; init; }

        public MultiFightMonsterQuestObjective(QuestObjective questObjective) :
            base(questObjective)
        {
            List<string> parameters = questObjective.Parameters;

            MonsterId = parameters.Count > 0 && int.TryParse(parameters[0], out int monsterId) ? monsterId : 0;
            Quantity = parameters.Count > 1 && int.TryParse(parameters[1], out int quantity) ? quantity : 0;
        }

        public static new MultiFightMonsterQuestObjective Create(QuestObjective questObjective)
        {
            return new(questObjective);
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
