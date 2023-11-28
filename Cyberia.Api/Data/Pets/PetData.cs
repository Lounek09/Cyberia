using Cyberia.Api.Data.Items;
using Cyberia.Api.JsonConverters;

using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Pets;

public class PetData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("tmin")]
    public TimeSpan? MinFoodInterval { get; init; }

    [JsonPropertyName("tmax")]
    public TimeSpan? MaxFoodInterval { get; init; }

    [JsonPropertyName("f")]
    [JsonConverter(typeof(ReadOnlyCollectionConverter<PetFoodsData>))]
    public ReadOnlyCollection<PetFoodsData> Foods { get; init; }

    [JsonConstructor]
    internal PetData()
    {
        Foods = ReadOnlyCollection<PetFoodsData>.Empty;
    }

    public ItemData? GetItemData()
    {
        return DofusApi.Datacenter.ItemsData.GetItemDataById(Id);
    }
}
