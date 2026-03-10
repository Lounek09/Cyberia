using Cyberia.Api.Data.Items;

namespace Cyberia.Api.Factories.Effects.Interfaces;

public interface IItemTypeEffect
{
    int ItemTypeId { get; }

    ItemTypeData? GetItemTypeData();
}
