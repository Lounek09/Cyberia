using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Items.Localized;

internal sealed class ItemUnicStringLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public string Value { get; init; }

    [JsonConstructor]
    internal ItemUnicStringLocalizedData()
    {
        Value = string.Empty;
    }
}
