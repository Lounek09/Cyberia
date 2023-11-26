using Cyberia.Api.Data;

using System.Collections.ObjectModel;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public sealed record BringSoulToNpcQuestObjective : QuestObjective, IQuestObjective<BringSoulToNpcQuestObjective>
    {
        public int NpcId { get; init; }
        public int MonsterId { get; init; }
        public int Quantity { get; init; }

        private BringSoulToNpcQuestObjective(QuestObjectiveData questObjectiveData, int npcId, int monsterId, int quantity) :
            base(questObjectiveData)
        {
            NpcId = npcId;
            MonsterId = monsterId;
            Quantity = quantity;
        }

        public static BringSoulToNpcQuestObjective? Create(QuestObjectiveData questObjectiveData)
        {
            ReadOnlyCollection<string> parameters = questObjectiveData.Parameters;
            if (parameters.Count > 2 && int.TryParse(parameters[0], out int npcId) && int.TryParse(parameters[1], out int monsterId) && int.TryParse(parameters[2], out int quantity))
            {
                return new(questObjectiveData, npcId, monsterId, quantity);
            }

            return null;
        }

        public NpcData? GetNpcData()
        {
            return DofusApi.Datacenter.NpcsData.GetNpcDataById(NpcId);
        }

        public MonsterData? GetMonsterData()
        {
            return DofusApi.Datacenter.MonstersData.GetMonsterDataById(MonsterId);
        }

        public Description GetDescription()
        {
            string npcName = DofusApi.Datacenter.NpcsData.GetNpcNameById(NpcId);
            string monsterName = DofusApi.Datacenter.MonstersData.GetMonsterNameById(MonsterId);

            return GetDescription(npcName, monsterName, Quantity);
        }
    }
}

