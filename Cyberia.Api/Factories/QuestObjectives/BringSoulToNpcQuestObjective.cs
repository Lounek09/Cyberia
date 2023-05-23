using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public sealed record BringSoulToNpcQuestObjective : BasicQuestObjective
    {
        public int NpcId { get; init; }
        public int MonsterId { get; init; }
        public int Quantity { get; init; }

        public BringSoulToNpcQuestObjective(QuestObjective questObjective) :
            base(questObjective)
        {
            List<string> parameters = questObjective.Parameters;

            NpcId = parameters.Count > 0 && int.TryParse(parameters[0], out int npcId) ? npcId : 0;
            MonsterId = parameters.Count > 1 && int.TryParse(parameters[1], out int monsterId) ? monsterId : 0;
            Quantity = parameters.Count > 2 && int.TryParse(parameters[2], out int quantity) ? quantity : 0;
        }

        public static new BringSoulToNpcQuestObjective Create(QuestObjective questObjective)
        {
            return new(questObjective);
        }

        public Npcs? GetNpc()
        {
            return DofusApi.Instance.Datacenter.NpcsData.GetNpcById(NpcId);
        }

        public Monster? GetMonster()
        {
            return DofusApi.Instance.Datacenter.MonstersData.GetMonsterById(MonsterId);
        }

        public override string GetDescription()
        {
            string npcName = DofusApi.Instance.Datacenter.NpcsData.GetNpcNameById(NpcId);
            string monsterName = DofusApi.Instance.Datacenter.MonstersData.GetMonsterNameById(MonsterId);

            return GetDescriptionFromParameters(npcName, monsterName, Quantity.ToString());
        }
    }
}

