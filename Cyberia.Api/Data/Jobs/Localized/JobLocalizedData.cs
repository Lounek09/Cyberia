using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Jobs.Localized;

internal sealed class JobLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonConstructor]
    internal JobLocalizedData()
    {
        Name = string.Empty;
    }
}
