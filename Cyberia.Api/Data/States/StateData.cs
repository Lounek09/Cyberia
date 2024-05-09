using Cyberia.Api.Managers;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.States;

public sealed class StateData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("p")]
    [JsonInclude]
    internal int P { get; init; }

    [JsonPropertyName("d")]
    public bool Display { get; init; }

    [JsonPropertyName("s")]
    public string ShortName { get; init; }

    [JsonConstructor]
    internal StateData()
    {
        Name = string.Empty;
        ShortName = string.Empty;
    }

    public async Task<string> GetIconImagePathAsync(CdnImageSize size)
    {
        return await CdnManager.GetImagePathAsync("states", Id, size);
    }
}
