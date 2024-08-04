using Cyberia.Api.Data.Items;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.TTG;

public sealed class TTGFamilyData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("i")]
    public int Index { get; init; }

    [JsonPropertyName("o")]
    public int ItemId { get; init; }

    [JsonPropertyName("n")]
    public LocalizedString Name { get; init; }

    [JsonConstructor]
    internal TTGFamilyData()
    {
        Name = LocalizedString.Empty;
    }

    public ItemData? GetItemData()
    {
        return DofusApi.Datacenter.ItemsRepository.GetItemDataById(ItemId);
    }
}
