using Cyberia.Api.Data.Items;
using Cyberia.Api.Data.Maps;
using Cyberia.Api.Data.Npcs;
using Cyberia.Api.Data.Quests;

using System.Globalization;

namespace Cyberia.Api.Factories.QuestObjectives.Elements;

public sealed record BringItemToNpcInAreaQuestObjective : QuestObjective
{
    public int NpcId { get; init; }
    public int MapSubAreaId { get; init; }
    public int ItemId { get; init; }
    public int Quantity { get; init; }

    private BringItemToNpcInAreaQuestObjective(QuestObjectiveData questObjectiveData, int npcId, int itemId, int quantity, int mapSubAreaId)
        : base(questObjectiveData)
    {
        NpcId = npcId;
        ItemId = itemId;
        Quantity = quantity;
        MapSubAreaId = mapSubAreaId;
    }

    internal static BringItemToNpcInAreaQuestObjective? Create(QuestObjectiveData questObjectiveData)
    {
        var parameters = questObjectiveData.Parameters;
        if (parameters.Count > 3 && int.TryParse(parameters[0], out var npcId) && int.TryParse(parameters[1], out var itemId) && int.TryParse(parameters[2], out var quantity) && int.TryParse(parameters[3], out var mapSubAreaId))
        {
            return new(questObjectiveData, npcId, itemId, quantity, mapSubAreaId);
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

    public MapSubAreaData? GetMapSubAreaData()
    {
        return DofusApi.Datacenter.MapsRepository.GetMapSubAreaDataById(MapSubAreaId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var npcName = DofusApi.Datacenter.NpcsRepository.GetNpcNameById(NpcId, culture);
        var itemName = DofusApi.Datacenter.ItemsRepository.GetItemNameById(ItemId, culture);
        var mapSubAreaName = DofusApi.Datacenter.MapsRepository.GetMapSubAreaNameById(MapSubAreaId, culture);

        return GetDescription(culture, npcName, itemName, Quantity, mapSubAreaName);
    }
}
