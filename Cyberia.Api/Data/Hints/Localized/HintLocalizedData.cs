using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Hints.Localized;

public sealed class HintLocalizedData : IDofusData
{
    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonConstructor]
    internal HintLocalizedData()
    {
        Name = string.Empty;
    }
}
