using Cyberia.Api.Managers;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Effects;

public sealed class EffectData : IDofusData<int>, IComparable<EffectData>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("d")]
    [JsonInclude]
    public LocalizedString Description { get; internal set; }

    [JsonPropertyName("c")]
    public int CharacteristicId { get; init; }

    [JsonPropertyName("p")]
    public int Priority { get; init; }

    [JsonPropertyName("o")]
    public string Operator { get; init; }

    [JsonPropertyName("t")]
    public bool VisibleInTooltips { get; init; }

    [JsonPropertyName("j")]
    public bool DisplayableInDiceMode { get; init; }

    [JsonPropertyName("e")]
    public char? Element { get; init; }

    [JsonConstructor]
    internal EffectData()
    {
        Description = LocalizedString.Empty;
        Operator = string.Empty;
    }

    public async Task<string> GetIconImagePathAsync(CdnImageSize size)
    {
        return await CdnManager.GetImagePathAsync("effects", GetIconId(), size);
    }

    public string GetIconId()
    {
        return Element is null ? CharacteristicId.ToString() : Element.Value.ToString();
    }

    public int CompareTo(EffectData? other)
    {
        if (other is null)
        {
            return 1;
        }

        return Priority.CompareTo(other.Priority);
    }
}
