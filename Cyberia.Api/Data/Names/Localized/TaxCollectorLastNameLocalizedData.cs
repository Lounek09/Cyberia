using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Names.Localized;

internal sealed class TaxCollectorLastNameLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public string Name { get; init; }

    [JsonConstructor]
    internal TaxCollectorLastNameLocalizedData()
    {
        Name = string.Empty;
    }
}
