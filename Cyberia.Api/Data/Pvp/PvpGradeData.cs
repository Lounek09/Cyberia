using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Pvp;

public sealed class PvpGradeData : IDofusData
{
    [JsonPropertyName("nc")]
    public string ShortName { get; init; }

    [JsonPropertyName("nl")]
    public string Name { get; init; }

    [JsonConstructor]
    internal PvpGradeData()
    {
        ShortName = string.Empty;
        Name = string.Empty;
    }
}
