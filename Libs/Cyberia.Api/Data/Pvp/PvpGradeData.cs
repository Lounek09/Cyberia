using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Pvp;

public sealed class PvpGradeData : IDofusData
{
    [JsonPropertyName("nc")]
    public LocalizedString ShortName { get; init; }

    [JsonPropertyName("nl")]
    public LocalizedString Name { get; init; }

    [JsonConstructor]
    internal PvpGradeData()
    {
        ShortName = LocalizedString.Empty;
        Name = LocalizedString.Empty;
    }
}
