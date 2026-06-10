using Cyberia.Api.Data.Items;
using Cyberia.Api.Data.Npcs;
using Cyberia.Api.Data.Quests;

using System.Globalization;

namespace Cyberia.Api.Factories.QuestObjectives.Elements;

public sealed record ShowItemToNpcQuestObjective : QuestObjective
{
    public int NpcId { get; init; }
    public int ItemId { get; init; }
    public int Quantity { get; init; }

    private ShowItemToNpcQuestObjective(QuestObjectiveData questObjectiveData, int npcId, int itemId, int quantity)
        : base(questObjectiveData)
    {
        NpcId = npcId;
        ItemId = itemId;
        Quantity = quantity;
    }

    internal static ShowItemToNpcQuestObjective? Create(QuestObjectiveData questObjectiveData)
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
        return DofusApi.Datacenter.NpcsRepository.GetNpcDataById(NpcId);
    }

    public ItemData? GetItemData()
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemDataById(ItemId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var npcName = DofusApi.Datacenter.NpcsRepository.GetNpcNameById(NpcId, culture);
        var itemName = DofusApi.Datacenter.ItemsRepository.GetItemNameById(ItemId, culture);

        return GetDescription(culture, npcName, itemName, Quantity);
    }
}
