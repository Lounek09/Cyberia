using Cyberia.Api.Data.Items;
using Cyberia.Api.Data.Npcs;
using Cyberia.Api.Data.Quests;

namespace Cyberia.Api.Factories.QuestObjectives;

public sealed record BringItemToNpcQuestObjective
    : QuestObjective, IQuestObjective
{
    public int NpcId { get; init; }
    public int ItemId { get; init; }
    public int Quantity { get; init; }

    private BringItemToNpcQuestObjective(QuestObjectiveData questObjectiveData, int npcId, int itemId, int quantity)
        : base(questObjectiveData)
    {
        NpcId = npcId;
        ItemId = itemId;
        Quantity = quantity;
    }

    internal static BringItemToNpcQuestObjective? Create(QuestObjectiveData questObjectiveData)
    {
        var parameters = questObjectiveData.Parameters;
        if (parameters.Count > 2 && int.TryParse(parameters[0], out var npcId) && int.TryParse(parameters[1], out var itemId) && int.TryParse(parameters[2], out var quantity))
        {
            return new(questObjectiveData, npcId, itemId, quantity);
        }

        return null;
    }

    public NpcData? GetNpcData()
    {
        return DofusApi.Datacenter.NpcsData.GetNpcDataById(NpcId);
    }

    public ItemData? GetItemData()
    {
        return DofusApi.Datacenter.ItemsData.GetItemDataById(ItemId);
    }

    public Description GetDescription()
    {
        var npcName = DofusApi.Datacenter.NpcsData.GetNpcNameById(NpcId);
        var itemName = DofusApi.Datacenter.ItemsData.GetItemNameById(ItemId);

        return GetDescription(npcName, itemName, Quantity);
    }
}
