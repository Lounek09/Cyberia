using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public sealed class BringSoulToNpcQuestObjective : BasicQuestObjective
    {
        public int NpcId { get; init; }
        public int MonsterId { get; init; }
        public int Quantity { get; init; }

        public BringSoulToNpcQuestObjective(QuestObjective questObjective, int npcId, int monsterId, int quantity) :
            base(questObjective)
        {
            NpcId = npcId;
            MonsterId = monsterId;
            Quantity = quantity;
        }

        public static new BringSoulToNpcQuestObjective? Create(QuestObjective questObjective)
        {
            if (questObjective.Parameters.Count > 2 &&
                int.TryParse(questObjective.Parameters[0], out int npcId) &&
                int.TryParse(questObjective.Parameters[1], out int monsterId) &&
                int.TryParse(questObjective.Parameters[2], out int quantity))
                return new(questObjective, npcId, monsterId, quantity);

            return null;
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

