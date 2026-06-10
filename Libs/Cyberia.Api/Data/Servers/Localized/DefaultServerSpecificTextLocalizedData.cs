using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Servers.Localized;

internal sealed class DefaultServerSpecificTextLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("d")]
    public string Description { get; init; }

    [JsonConstructor]
    internal DefaultServerSpecificTextLocalizedData()
    {
        Description = string.Empty;
    }
}
