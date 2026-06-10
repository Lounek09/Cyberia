using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Hints.Localized;

internal sealed class HintCategoryLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonConstructor]
    internal HintCategoryLocalizedData()
    {
        Name = string.Empty;
    }
}
