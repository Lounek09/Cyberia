using Cyberia.Api.Data.Items;
using Cyberia.Api.Data.Quests;

using System.Globalization;

namespace Cyberia.Api.Factories.QuestObjectives.Elements;

public sealed record UseItemQuestObjective : QuestObjective
{
    public int ItemId { get; init; }

    private UseItemQuestObjective(QuestObjectiveData questObjectiveData, int itemId)
        : base(questObjectiveData)
    {
        ItemId = itemId;
    }

    internal static UseItemQuestObjective? Create(QuestObjectiveData questObjectiveData)
    {
        var parameters = questObjectiveData.Parameters;
        if (parameters.Count > 0 && int.TryParse(parameters[0], out var itemId))
        {
            return new(questObjectiveData, itemId);
        }

        return null;
    }

    public ItemData? GetItemData()
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemDataById(ItemId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var itemName = DofusApi.Datacenter.ItemsRepository.GetItemNameById(ItemId, culture);

        return GetDescription(culture, itemName);
    }
}
