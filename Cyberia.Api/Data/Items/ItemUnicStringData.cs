using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Items;

public sealed class ItemUnicStringData
    : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public string Value { get; init; }

    [JsonConstructor]
    internal ItemUnicStringData()
    {
        Value = string.Empty;
    }
}
