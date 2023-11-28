using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Items;

public sealed class ItemSuperTypeData : IDofusData<int>
{
    public const int SUPER_TYPE_QUEST = 14;

    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    [JsonInclude]
    internal bool V { get; init; }

    [JsonIgnore]
    public ReadOnlyCollection<int> SlotsId { get; internal set; }

    [JsonConstructor]
    internal ItemSuperTypeData()
    {
        SlotsId = ReadOnlyCollection<int>.Empty;
    }
}
