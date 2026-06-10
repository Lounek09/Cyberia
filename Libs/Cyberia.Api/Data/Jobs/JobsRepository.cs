using Cyberia.Api.Data.Jobs.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Primitives;

using System.Collections.Frozen;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Jobs;

public sealed class JobsRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "jobs.json";

    [JsonPropertyName("J")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, JobData>))]
    public FrozenDictionary<int, JobData> Jobs { get; init; }

    [JsonConstructor]
    internal JobsRepository()
    {
        Jobs = FrozenDictionary<int, JobData>.Empty;
    }

    public JobData? GetJobDataById(int id)
    {
        Jobs.TryGetValue(id, out var jobData);
        return jobData;
    }

    public string GetJobNameById(int id, Language language)
    {
        return GetJobNameById(id, language.ToCulture());
    }

    public string GetJobNameById(int id, CultureInfo? culture = null)
    {
        var jobData = GetJobDataById(id);

        return jobData is null
            ? Translation.UnknownData(id, culture)
            : jobData.Name.ToString(culture);
    }

    protected override void LoadLocalizedData(LangsIdentifier identifier)
    {
        var twoLetterISOLanguageName = identifier.Language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<JobsLocalizedRepository>(identifier);

        foreach (var jobLocalizedData in localizedRepository.Jobs)
        {
            var jobData = GetJobDataById(jobLocalizedData.Id);
            jobData?.Name.TryAdd(twoLetterISOLanguageName, jobLocalizedData.Name);
        }
    }
}
