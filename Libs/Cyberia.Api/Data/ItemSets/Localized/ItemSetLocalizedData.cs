using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.ItemSets.Localized;

internal sealed class ItemSetLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonConstructor]
    internal ItemSetLocalizedData()
    {
        Name = string.Empty;
    }
}
