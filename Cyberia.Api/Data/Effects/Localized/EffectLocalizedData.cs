using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Effects.Localized;

internal sealed class EffectLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("d")]
    [JsonInclude]
    public string Description { get; internal set; }

    [JsonConstructor]
    internal EffectLocalizedData()
    {
        Description = string.Empty;
    }
}
