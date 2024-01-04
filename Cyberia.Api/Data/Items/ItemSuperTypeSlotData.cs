using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Items;

internal sealed class ItemSuperTypeSlotData
    : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public IReadOnlyList<int> SlotsId { get; init; }

    [JsonConstructor]
    internal ItemSuperTypeSlotData()
    {
        SlotsId = [];
    }
}
