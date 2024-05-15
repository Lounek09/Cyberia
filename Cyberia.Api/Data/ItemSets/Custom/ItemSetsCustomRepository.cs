using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.ItemSets.Custom;

internal sealed class ItemSetsCustomRepository : IDofusRepository
{
    [JsonPropertyName("CIS")]
    public IReadOnlyList<ItemSetCustomData> ItemSetsCustom { get; init; }

    [JsonConstructor]
    internal ItemSetsCustomRepository()
    {
        ItemSetsCustom = [];
    }
}
