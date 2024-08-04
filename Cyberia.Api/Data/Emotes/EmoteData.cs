using Cyberia.Api.Managers;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Emotes;

public sealed class EmoteData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public LocalizedString Name { get; init; }

    [JsonPropertyName("s")]
    public string Shortcut { get; init; }

    [JsonConstructor]
    internal EmoteData()
    {
        Name = LocalizedString.Empty;
        Shortcut = string.Empty;
    }

    public async Task<string> GetIconImagePathAsync(CdnImageSize size)
    {
        return await CdnManager.GetImagePathAsync("emotes", Id, size);
    }
}
