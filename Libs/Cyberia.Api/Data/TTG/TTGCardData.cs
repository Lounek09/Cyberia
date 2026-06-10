using Cyberia.Api.Data.Items;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.TTG;

public sealed class TTGCardData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("i")]
    public int Index { get; init; }

    [JsonPropertyName("o")]
    public int ItemId { get; init; }

    [JsonPropertyName("e")]
    public int TTGEntityId { get; init; }

    [JsonPropertyName("v")]
    public int Variant { get; init; }

    [JsonConstructor]
    internal TTGCardData() { }

    public ItemData? GetItemData()
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemDataById(ItemId);
    }

    public TTGEntityData? GetTTGEntityData()
    {
        return DofusApi.Datacenter.TTGRepository.GetTTGEntityDataById(ItemId);
    }
}
