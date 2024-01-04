using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.States;

public sealed class StateData
    : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("p")]
    [JsonInclude]
    internal int P { get; init; }

    [JsonConstructor]
    internal StateData()
    {
        Name = string.Empty;
    }

    public string GetImagePath()
    {
        return $"{DofusApi.Config.CdnUrl}/images/states/{Id}.png";
    }
}
