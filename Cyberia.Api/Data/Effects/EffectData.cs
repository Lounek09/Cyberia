using Cyberia.Api.Managers;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Effects;

public sealed class EffectData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("d")]
    [JsonInclude]
    public string Description { get; internal set; }

    [JsonPropertyName("c")]
    public int CharacteristicId { get; init; }

    [JsonPropertyName("o")]
    public string Operator { get; init; }

    [JsonPropertyName("t")]
    public bool ShowInTooltip { get; init; }

    [JsonPropertyName("j")]
    public bool ShowInDiceModePossible { get; init; }

    [JsonPropertyName("e")]
    public string Element { get; init; }

    [JsonConstructor]
    internal EffectData()
    {
        Description = string.Empty;
        Operator = string.Empty;
        Element = string.Empty;
    }

    public async Task<string> GetIconImagePathAsync(CdnImageSize size)
    {
        return await CdnManager.GetImagePathAsync("effects", Id, size);
    }
}
