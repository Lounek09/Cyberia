using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.TTG.Localized;

internal sealed class TTGEntityLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonConstructor]
    internal TTGEntityLocalizedData()
    {
        Name = string.Empty;
    }
}
