using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Pvp.Localized;

internal sealed class PvpGradeLocalizedData : IDofusData
{
    [JsonPropertyName("nc")]
    public string ShortName { get; init; }

    [JsonPropertyName("nl")]
    public string Name { get; init; }

    [JsonConstructor]
    internal PvpGradeLocalizedData()
    {
        ShortName = string.Empty;
        Name = string.Empty;
    }
}
