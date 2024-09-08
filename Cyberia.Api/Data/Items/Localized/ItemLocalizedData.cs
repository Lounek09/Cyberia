using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Items.Localized;

internal sealed class ItemLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("nn")]
    public string NormalizedName { get; init; }

    [JsonPropertyName("d")]
    public string Description { get; init; }

    [JsonConstructor]
    internal ItemLocalizedData()
    {
        Name = string.Empty;
        NormalizedName = string.Empty;
        Description = string.Empty;
    }
}
