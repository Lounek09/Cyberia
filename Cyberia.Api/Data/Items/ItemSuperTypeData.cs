using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Items;

public sealed class ItemSuperTypeData : IDofusData<int>
{
    public const int Quest = 14;

    public static readonly IReadOnlyList<int> EnhanceableSuperTypes = [1, 2, 3, 4, 5, 10, 11];

    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    [JsonInclude]
    internal bool V { get; init; }

    [JsonIgnore]
    public IReadOnlyList<int> SlotsId { get; internal set; }

    [JsonConstructor]
    internal ItemSuperTypeData()
    {
        SlotsId = [];
    }
}
