using Cyberia.Api.Data.Items;
using Cyberia.Api.Data.Quests;

using System.Globalization;

namespace Cyberia.Api.Factories.QuestObjectives.Elements;

public sealed record CreateMagicFragmentQuestObjective : QuestObjective
{
    public int ItemId { get; init; }
    public int Quantity { get; init; }

    private CreateMagicFragmentQuestObjective(QuestObjectiveData questObjectiveData, int itemId, int quantity)
        : base(questObjectiveData)
    {
        ItemId = itemId;
        Quantity = quantity;
    }

    internal static CreateMagicFragmentQuestObjective? Create(QuestObjectiveData questObjectiveData)
    {
        var parameters = questObjectiveData.Parameters;
        if (parameters.Count > 2 && int.TryParse(parameters[0], out var itemId) && int.TryParse(parameters[1], out var quantity))
        {
            return new(questObjectiveData, itemId, quantity);
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

        return GetDescription(culture, Quantity, itemName);
    }
}
