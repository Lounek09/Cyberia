using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Jobs.Localized;

internal sealed class JobsLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => JobsRepository.FileName;

    [JsonPropertyName("J")]
    public IReadOnlyList<JobLocalizedData> Jobs { get; init; }

    [JsonConstructor]
    internal JobsLocalizedRepository()
    {
        Jobs = ReadOnlyCollection<JobLocalizedData>.Empty;
    }
}
