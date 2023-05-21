using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public sealed record BringItemToNpcQuestObjective(QuestObjective QuestObjective, int NpcId, int ItemId, int Quantity) :
        BasicQuestObjective(QuestObjective)
    {
        public static new BringItemToNpcQuestObjective? Create(QuestObjective questObjective)
        {
            if (questObjective.Parameters.Count > 2 &&
                int.TryParse(questObjective.Parameters[0], out int npcId) &&
                int.TryParse(questObjective.Parameters[1], out int itemId) &&
                int.TryParse(questObjective.Parameters[2], out int quantity))
                return new(questObjective, npcId, itemId, quantity);

            return null;
        }

        public Npcs? GetNpc()
        {
            return DofusApi.Instance.Datacenter.NpcsData.GetNpcById(NpcId);
        }

        public Item? GetItem()
        {
            return DofusApi.Instance.Datacenter.ItemsData.GetItemById(ItemId);
        }

        public override string GetDescription()
        {
            string npcName = DofusApi.Instance.Datacenter.NpcsData.GetNpcNameById(NpcId);
            string itemName = DofusApi.Instance.Datacenter.ItemsData.GetItemNameById(ItemId);

            return GetDescriptionFromParameters(npcName, itemName, Quantity.ToString());
        }
    }
}
