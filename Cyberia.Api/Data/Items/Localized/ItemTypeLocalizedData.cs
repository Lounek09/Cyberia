using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Items.Localized;

internal sealed class ItemTypeLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonConstructor]
    internal ItemTypeLocalizedData()
    {
        Name = string.Empty;
    }
}
