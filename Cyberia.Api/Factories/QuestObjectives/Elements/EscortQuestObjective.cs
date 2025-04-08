using Cyberia.Api.Data.Items;
using Cyberia.Api.Data.Npcs;
using Cyberia.Api.Data.Quests;

using System.Globalization;

namespace Cyberia.Api.Factories.QuestObjectives.Elements;

public sealed record EscortQuestObjective : QuestObjective
{
    public int NpcId { get; init; }
    public int ItemId { get; init; }

    private EscortQuestObjective(QuestObjectiveData questObjectiveData, int npcId, int itemId)
        : base(questObjectiveData)
    {
        NpcId = npcId;
        ItemId = itemId;
    }

    internal static EscortQuestObjective? Create(QuestObjectiveData questObjectiveData)
    {
        var parameters = questObjectiveData.Parameters;
        if (parameters.Count > 1 && int.TryParse(parameters[0], out var npcId) && int.TryParse(parameters[1], out var itemId))
        {
            return new(questObjectiveData, npcId, itemId);
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

        return GetDescription(culture, npcName, itemName);
    }
}
