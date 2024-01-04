using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Hints;

public sealed class HintCategoryData
    : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("c")]
    public string Color { get; init; }

    [JsonConstructor]
    internal HintCategoryData()
    {
        Name = string.Empty;
        Color = string.Empty;
    }
}
