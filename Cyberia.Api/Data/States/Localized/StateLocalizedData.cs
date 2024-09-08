using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.States.Localized;

internal sealed class StateLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("s")]
    public string ShortName { get; init; }

    [JsonConstructor]
    internal StateLocalizedData()
    {
        Name = string.Empty;
        ShortName = string.Empty;
    }
}
