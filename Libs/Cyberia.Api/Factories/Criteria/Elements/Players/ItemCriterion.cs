using Cyberia.Api.Data.Items;

using System.Globalization;

namespace Cyberia.Api.Factories.Criteria.Elements.Players;

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

    internal static ItemCriterion? Create(string id, char @operator, params ReadOnlySpan<string> parameters)
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

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var itemName = DofusApi.Datacenter.ItemsRepository.GetItemNameById(ItemId, culture);

        return GetDescription(culture, itemName, Quantity);
    }
}
