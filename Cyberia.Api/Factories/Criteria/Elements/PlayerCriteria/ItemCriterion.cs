using Cyberia.Api.Data.Items;

namespace Cyberia.Api.Factories.Criteria;

public sealed record ItemCriterion
    : Criterion, ICriterion
{
    public int ItemId { get; init; }

    private ItemCriterion(string id, char @operator, int emoteId)
        : base(id, @operator)
    {
        ItemId = emoteId;
    }

    internal static ItemCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var itemId))
        {
            return new(id, @operator, itemId);
        }

        return null;
    }

    public ItemData? GetItemData()
    {
        return DofusApi.Datacenter.ItemsData.GetItemDataById(ItemId);
    }

    protected override string GetDescriptionName()
    {
        return $"Criterion.Item.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        var itemName = DofusApi.Datacenter.ItemsData.GetItemNameById(ItemId);

        return GetDescription(itemName);
    }
}
