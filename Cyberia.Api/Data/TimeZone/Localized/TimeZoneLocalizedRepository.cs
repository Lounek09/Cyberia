using Cyberia.Api.JsonConverters;

using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.TimeZone.Localized;

internal sealed class TimeZoneLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => TimeZoneRepository.FileName;

    [JsonPropertyName("T.m")]
    [JsonConverter(typeof(ReadOnlyDictionaryFromArrayConverter<int, string>))]
    public IReadOnlyDictionary<int, string> StartDayOfMonths { get; set; }

    [JsonConstructor]
    internal TimeZoneLocalizedRepository()
    {
        StartDayOfMonths = ReadOnlyDictionary<int, string>.Empty;
    }
}
