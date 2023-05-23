using Cyberia.Api.DatacenterNS;

namespace Cyberia.Api.Factories.QuestObjectives
{
    public sealed record BringItemToNpcQuestObjective : BasicQuestObjective
    {
        public int NpcId { get; init; }
        public int ItemId { get; init; }
        public int Quantity { get; init; }

        public BringItemToNpcQuestObjective(QuestObjective questObjective) :
            base(questObjective)
        {
            List<string> parameters = questObjective.Parameters;

            NpcId = parameters.Count > 0 && int.TryParse(parameters[0], out int npcId) ? npcId : 0;
            ItemId = parameters.Count > 1 && int.TryParse(parameters[1], out int itemId) ? itemId : 0;
            Quantity = parameters.Count > 2 && int.TryParse(parameters[2], out int quantity) ? quantity : 0;
        }

        public static new BringItemToNpcQuestObjective Create(QuestObjective questObjective)
        {
            return new(questObjective);
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
