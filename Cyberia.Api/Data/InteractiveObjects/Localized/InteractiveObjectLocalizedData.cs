using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.InteractiveObjects.Localized;

internal sealed class InteractiveObjectLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonConstructor]
    internal InteractiveObjectLocalizedData()
    {
        Name = string.Empty;
    }
}
