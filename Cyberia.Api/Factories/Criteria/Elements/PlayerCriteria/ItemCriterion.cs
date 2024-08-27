using Cyberia.Api.Data.Items;

namespace Cyberia.Api.Factories.Criteria;

public sealed record ItemCriterion : Criterion
{
    public int ItemId { get; init; }
    public int Quantity { get; init; }

    private ItemCriterion(string id, char @operator, int itemId, int quantity)
        : base(id, @operator)
    {
        ItemId = itemId;
        Quantity = quantity;
    }

    internal static ItemCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 1 && int.TryParse(parameters[0], out var itemId) && int.TryParse(parameters[1], out var quantity))
        {
            return new(id, @operator, itemId, quantity);
        }

        if (parameters.Length > 0 && int.TryParse(parameters[0], out itemId))
        {
            return new(id, @operator, itemId, 0);
        }

        return null;
    }

    public ItemData? GetItemData()
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemDataById(ItemId);
    }

    protected override string GetDescriptionKey()
    {
        if (Quantity > 0)
        {
            return $"Criterion.Item.{GetOperatorDescriptionKey()}.Quantity";
        }

        return $"Criterion.Item.{GetOperatorDescriptionKey()}";
    }

    public override DescriptionString GetDescription()
    {
        var itemName = DofusApi.Datacenter.ItemsRepository.GetItemNameById(ItemId);

        return GetDescription(itemName, Quantity);
    }
}
