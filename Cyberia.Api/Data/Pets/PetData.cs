using Cyberia.Api.Data.Items;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Pets;

public sealed class PetData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("tmin")]
    public TimeSpan? MinFoodInterval { get; init; }

    [JsonPropertyName("tmax")]
    public TimeSpan? MaxFoodInterval { get; init; }

    [JsonPropertyName("f")]
    public IReadOnlyList<PetFoodsData> Foods { get; init; }

    [JsonConstructor]
    internal PetData()
    {
        Foods = [];
    }

    public ItemData? GetItemData()
    {
        return DofusApi.Datacenter.ItemsData.GetItemDataById(Id);
    }
}
