using Cyberia.Api.JsonConverters;

using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Items;

internal sealed class ItemSuperTypeSlotData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    [JsonConverter(typeof(ReadOnlyCollectionConverter<int>))]
    public ReadOnlyCollection<int> SlotsId { get; init; }

    [JsonConstructor]
    internal ItemSuperTypeSlotData()
    {
        SlotsId = ReadOnlyCollection<int>.Empty;
    }
}