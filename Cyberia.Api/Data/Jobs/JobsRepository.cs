using Cyberia.Api.Data.Jobs.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
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

    public string GetJobNameById(int id)
    {
        var jobData = GetJobDataById(id);

        return jobData is null
            ? Translation.Format(ApiTranslations.Unknown_Data, id)
            : jobData.Name;
    }

    protected override void LoadLocalizedData(LangType type, LangLanguage language)
    {
        var twoLetterISOLanguageName = language.ToCulture().TwoLetterISOLanguageName;
        var localizedRepository = DofusLocalizedRepository.Load<JobsLocalizedRepository>(type, language);

        foreach (var jobLocalizedData in localizedRepository.Jobs)
        {
            var jobData = GetJobDataById(jobLocalizedData.Id);
            jobData?.Name.Add(twoLetterISOLanguageName, jobLocalizedData.Name);
        }
    }
}
