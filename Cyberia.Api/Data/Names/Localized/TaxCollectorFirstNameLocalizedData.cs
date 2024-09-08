using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Names.Localized;

internal sealed class TaxCollectorFirstNameLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public string Name { get; init; }

    [JsonConstructor]
    internal TaxCollectorFirstNameLocalizedData()
    {
        Name = string.Empty;
    }
}
