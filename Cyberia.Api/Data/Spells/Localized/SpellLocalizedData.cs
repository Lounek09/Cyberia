using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Spells.Localized;

internal sealed class SpellLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("d")]
    public string Description { get; init; }

    [JsonConstructor]
    internal SpellLocalizedData()
    {
        Name = string.Empty;
        Description = string.Empty;
    }
}
