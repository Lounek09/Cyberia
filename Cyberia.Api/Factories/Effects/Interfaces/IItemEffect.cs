using Cyberia.Api.Data.Items;

namespace Cyberia.Api.Factories.Effects.Interfaces;

public interface IItemEffect
{
    int ItemId { get; }

    ItemData? GetItemData();
}
