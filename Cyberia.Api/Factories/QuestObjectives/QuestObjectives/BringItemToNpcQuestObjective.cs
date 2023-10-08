using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public sealed record BringItemToNpcQuestObjective : QuestObjective, IQuestObjective<BringItemToNpcQuestObjective>
    {
        public int NpcId { get; init; }
        public int ItemId { get; init; }
        public int Quantity { get; init; }

        private BringItemToNpcQuestObjective(QuestObjectiveData questObjectiveData, int npcId, int itemId, int quantity) :
            base(questObjectiveData)
        {
            NpcId = npcId;
            ItemId = itemId;
            Quantity = quantity;
        }

        public static BringItemToNpcQuestObjective? Create(QuestObjectiveData questObjectiveData)
        {
            List<string> parameters = questObjectiveData.Parameters;
            if (parameters.Count > 2 && int.TryParse(parameters[0], out int npcId) && int.TryParse(parameters[1], out int itemId) && int.TryParse(parameters[2], out int quantity))
                return new(questObjectiveData, npcId, itemId, quantity);

            return null;
        }

        public NpcData? GetNpcData()
        {
            return DofusApi.Instance.Datacenter.NpcsData.GetNpcDataById(NpcId);
        }

        public ItemData? GetItemData()
        {
            return DofusApi.Instance.Datacenter.ItemsData.GetItemDataById(ItemId);
        }

        public Description GetDescription()
        {
            string npcName = DofusApi.Instance.Datacenter.NpcsData.GetNpcNameById(NpcId);
            string itemName = DofusApi.Instance.Datacenter.ItemsData.GetItemNameById(ItemId);

            return GetDescription(npcName, itemName, Quantity);
        }
    }
}
