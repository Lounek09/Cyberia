using Cyberia.Api.Utils;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.States;

public sealed class StateData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public LocalizedString Name { get; init; }

    [JsonPropertyName("p")]
    [JsonInclude]
    internal int P { get; init; }

    [JsonPropertyName("d")]
    public bool Display { get; init; }

    [JsonPropertyName("s")]
    public LocalizedString ShortName { get; init; }

    [JsonConstructor]
    internal StateData()
    {
        Name = LocalizedString.Empty;
        ShortName = LocalizedString.Empty;
    }

    public async Task<string> GetIconImagePathAsync(CdnImageSize size)
    {
        return await ImageUrlProvider.GetImagePathAsync("states", Id, size);
    }
}
